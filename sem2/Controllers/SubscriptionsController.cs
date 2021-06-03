using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Authentication.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using sem2.Infrastructure.Services;
using sem2.ViewModels.SubscriptionModels;

namespace sem2.Controllers
{
    public class SubscriptionsController : Controller
    {
        private PermissionService _permissionService;
        private ApplicationContext _dbContext;
        private IPaymentService _paymentService;

        public SubscriptionsController(PermissionService permissionService, ApplicationContext dbContext, IPaymentService paymentService)
        {
            _permissionService = permissionService;
            _dbContext = dbContext;
            _paymentService = paymentService;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var userSubs = _permissionService.GetUserSubscriptionsWithDate();
            var userSubsDict = new Dictionary<int, DateTime>();
            await foreach(var sub in userSubs)
                userSubsDict.Add(sub.plan.Id, sub.end);
            var model = new SubscriptionCatalogViewModel();
            model.Subscriptions = _dbContext.SubscriptionPlans.Select(sub => new SubscriptionViewModel()
            {
                Id = sub.Id,
                Name = sub.Name,
                Description = sub.Description,
                ImagePath = sub.Image.ImagePath,
                Price = sub.Price,
                Duration = sub.Duration
            }).ToList();
            
            foreach(var sub in model.Subscriptions)
                if (userSubsDict.ContainsKey(sub.Id))
                {
                    sub.IsUserSubscribed = true;
                    sub.EndPeriodDate = userSubsDict[sub.Id];
                }

            return View(model);
        }

        [HttpGet("~/subscriptionInfo/{planId}")]
        [Authorize]
        public IActionResult Subscribe(int planId)
        {
            var sub = _dbContext.SubscriptionPlans
                .Where(plan => plan.Id == planId)
                .Select(plan => new
                {
                    name = plan.Name,
                    price = plan.Price,
                    duration = plan.Duration
                })
                .FirstOrDefault();

            if (sub == null)
                return BadRequest();

            return Json(sub);
        }

        [HttpPost("~/subscribe/{planId}")]
        [Authorize]
        public async Task<IActionResult> Subscribe(int planId, CreditCardInfo creditCardInfo)
        {
            if (!ModelState.IsValid)
                return BadRequest(string.Join("; ",
                    ModelState.Values
                        .SelectMany(state => state.Errors)
                        .Select(error => error.ErrorMessage)));

            var hasPlan = await _permissionService.HasSubscriptionPlan(planId);
            if (hasPlan)
                return BadRequest("Пользователь уже подписан на данный план");
            
            var priceResult = _permissionService.GetPlanPrice(planId);
            if (!priceResult.TryGetValue(out var price))
                return BadRequest(priceResult.ErrorMessage);
            
            var paymentResult = _paymentService.Pay(price, creditCardInfo);
            if (!paymentResult.IsSuccessful)
                return BadRequest(paymentResult.ErrorMessage);

            var subscribeResult = await _permissionService.AddPlanToUser(planId);
            if (!subscribeResult.IsSuccessful)
                return BadRequest(subscribeResult.ErrorMessage);
            
            return Ok(Url.Action("Index"));
        }
    }
}