using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Authentication.Infrastructure;
using Authentication.Services;
using DomainModels;

namespace sem2.Infrastructure.Services
{
    public class PermissionService
    {
        private readonly ApplicationContext _dbContext;
        private readonly UserManager _userManager;

        public PermissionService(ApplicationContext dbContext, UserManager userManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
        }

        public async Task<bool> HasAllPermissions(params string[] permissionNames)
        {
            var userIdResult = _userManager.GetUserId();
            if (!userIdResult.IsSuccessful)
                return false;

            var userId = userIdResult.Value;
            var userPermissionsLookup = _dbContext.Users.Where(user => user.Id == userId)
                .SelectMany(user => user.UserPermissions)
                .ToList()
                .ToLookup(
                    perm => perm.PermissionName,
                    perm => perm);

            var result = true;
            foreach (var perm in permissionNames)
            {
                if (!userPermissionsLookup.Contains(perm))
                    result = false;
                else
                {
                    var userPermissions = userPermissionsLookup[perm];
                    var allRemoved = await CheckUserPermissions(userPermissions);

                    if (allRemoved)
                        result = false;
                }
            }

            await _dbContext.SaveChangesAsync();
            return result;
        }

        private IEnumerable<SubscriptionPlan> GetUserSubscriptions(int userId)
        {
            var result = _dbContext.Users
                .Where(user => user.Id == userId)
                .SelectMany(user => user.UserPermissions)
                .Select(perm => perm.PermissionProvider)
                .ToList()
                .GroupBy(plan => plan.Id)
                .Select(plans => plans.First())
                .ToList();

            return result;
        }

        public async IAsyncEnumerable<(SubscriptionPlan plan, DateTime end)> GetUserSubscriptionsWithDate()
        {
            var userIdResult = _userManager.GetUserId();
            if (!userIdResult.IsSuccessful)
                yield break;

            var userId = userIdResult.Value;
            var subs = GetUserSubscriptions(userId);
            foreach (var sub in subs)
            {
                var dateEnd = await GetUserSubscriptionExpireDate(userId, sub.Id);
                if(dateEnd != null && dateEnd.Value > DateTime.Now)
                    yield return (sub, dateEnd.Value);
            }
        }

        public async Task<DateTime?> GetUserSubscriptionExpireDate(int userId, int planId)
        {
            var userPermissions = _dbContext.Users
                .Where(user => user.Id == userId)
                .SelectMany(user => user.UserPermissions)
                .Where(perm => perm.PermissionProviderId == planId)
                .ToList();

            var maxDate = DateTime.MinValue;
            foreach(var perm in userPermissions)
                if (perm.PeriodEnd > maxDate)
                    maxDate = perm.PeriodEnd;

            return maxDate;
        }
        
        public async Task<bool> HasSubscriptionPlan(int planId)
        {
            var userIdResult = _userManager.GetUserId();
            if (!userIdResult.IsSuccessful)
                return false;

            var userId = userIdResult.Value;

            var userPermissions = _dbContext.Users
                .Where(user => user.Id == userId)
                .SelectMany(user => user.UserPermissions)
                .Where(perm => perm.PermissionProviderId == planId)
                .ToList();

            return !(await CheckUserPermissions(userPermissions));
        }

        public Result<decimal> GetPlanPrice(int planId)
        {
            var subPlan = _dbContext.SubscriptionPlans
                .FirstOrDefault(plan => plan.Id == planId);
            if(subPlan == null)
                return Result.Failure<decimal>("План не найден.");

            return Result.Success(subPlan.Price);
        }

        public async Task<Result> AddPlanToUser(int planId)
        {
            var userIdResult = _userManager.GetUserId();
            if (!userIdResult.IsSuccessful)
                return Result.Failure(userIdResult.ErrorMessage);

            var userId = userIdResult.Value;
            var user = _dbContext.Users.FirstOrDefault(u => u.Id == userId);
            if (user == null)
                return Result.Failure("Пользователь не найден");

            var subPlan = _dbContext.SubscriptionPlans
                .Where(plan => plan.Id == planId)
                .Select(plan => new
                {
                    PermissionsToGrant = plan.ProvidedPermissions,
                    plan.Duration
                })
                .FirstOrDefault();
            
            if (subPlan == null)
                return Result.Failure("План не найден");

            foreach (var perm in subPlan.PermissionsToGrant)
            {
                var userPermission = new UserPermission()
                {
                    PeriodStart = DateTime.Now,
                    PeriodEnd = DateTime.Now.AddDays(subPlan.Duration),
                    Permission = perm,
                    PermissionProviderId = planId,
                    UserId = userId
                };

                user.UserPermissions.Add(userPermission);
            }

            await _dbContext.SaveChangesAsync();
            return Result.Success();
        }

        public async Task<Result> RemovePlanFromUser(int planId)
        {
            var userIdResult = _userManager.GetUserId();
            if (!userIdResult.IsSuccessful)
                return Result.Failure(userIdResult.ErrorMessage);

            var userId = userIdResult.Value;
            var permissionsToRemove = _dbContext.UserPermissions
                .Where(perm => perm.UserId == userId && perm.PermissionProviderId == planId)
                .ToList();

            _dbContext.UserPermissions.RemoveRange(permissionsToRemove);
            await _dbContext.SaveChangesAsync();
            return Result.Success();
        }

        private async Task<bool> CheckUserPermissions(IEnumerable<UserPermission> userPermissions)
        {
            var allRemoved = true;
            foreach(var userPermission in userPermissions)
                if (userPermission.PeriodEnd <= DateTime.Now)
                    _dbContext.UserPermissions.Remove(userPermission);
                else allRemoved = false;

            await _dbContext.SaveChangesAsync();
            return allRemoved;
        }
    }
}