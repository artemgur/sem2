using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Authentication.Infrastructure;
using Authentication.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using sem2.ViewModels.ProfileModels;
using sem2_FSharp;

namespace sem2.Controllers
{
    public class ProfileController : Controller
    {
        private readonly ApplicationContext _dbContext;
        private readonly ILogger<ProfileController> _logger;
        private readonly IWebHostEnvironment _appEnvironment;
        private readonly AuthenticationService _authenticationService;
        private readonly UserManager _userManager;

        public ProfileController(ILogger<ProfileController> logger, ApplicationContext dbContext, IWebHostEnvironment appEnvironment, AuthenticationService authenticationService, UserManager userManager)
        {
            _logger = logger;
            _dbContext = dbContext;
            _appEnvironment = appEnvironment;
            _authenticationService = authenticationService;
            _userManager = userManager;
        }

        [Route("~/profile/{userId:int?}")]
        public IActionResult Profile(int userId = -1)
        {
            if (userId == -1)
            {
                var userIdResult = _userManager.GetUserId();
                if (userIdResult.IsSuccessful)
                    userId = userIdResult.Value;
                else return RedirectToAction("Login", "Account");
            }

            var model = _dbContext.Users.Where(user => user.Id == userId)
                .Select(user => new UserViewModel()
            {
                Email = user.Email,
                Id = user.Id,
                FirstName = user.FirstName,
                Surname = user.Surname,
                ImagePath = user.Image.ImagePath
            }).FirstOrDefault();

            if (model == null)
                return RedirectToAction("Login", "Account");
            
            return View(model);
        }

        // [HttpGet]
        // [Authorize]
        // public IActionResult ProfileEdit()
        // {
        //     var user = _dbContext.Users.ById(User.GetId()).FirstOrDefault();
        //     if (user == null)
        //         return RedirectToAction("Index", "Home");
        //     
        //     var model = new UserViewModel()
        //     {
        //         Email = user.Email,
        //         Id = user.Id,
        //         Name = user.Name,
        //         Surname = user.Surname
        //     };
        //
        //     return View(model);
        // }
        
        [HttpPost("~/profile/uploadImage")]
        [Authorize]
        public async Task<IActionResult> UploadImage(IFormFile image)
        {
            var userId = -1;
            var userIdResult = _userManager.GetUserId();
            if (userIdResult.IsSuccessful)
                userId = userIdResult.Value;
            else return BadRequest();
            
            var imageData = _dbContext.Users
                .Where(user => user.Id == userId)
                .Select(user => user.Image).FirstOrDefault();
            if (imageData == null)
                return RedirectToAction("Profile");
            
            var oldImagePath = Path.Combine(_appEnvironment.WebRootPath, imageData.ImagePath);

            var imageName = Path.GetFileNameWithoutExtension(Path.GetRandomFileName()) + Path.GetExtension(image.FileName);
            var virtualImagePath = Path.Combine("applicationData/profileImages", imageName);
            var imagePath = Path.Combine(_appEnvironment.WebRootPath, virtualImagePath);
            
            if (Path.GetFileNameWithoutExtension(oldImagePath) != "default" && System.IO.File.Exists(oldImagePath))
                System.IO.File.Delete(oldImagePath);

            await using (var fileStream = new FileStream(imagePath, FileMode.Create))
            {
                await image.CopyToAsync(fileStream);
            }

            imageData.ContentType = image.ContentType;
            imageData.ImagePath = virtualImagePath;

            await _dbContext.SaveChangesAsync();

            return RedirectToAction("Profile");
        }

        [HttpPost]
        public async Task<IActionResult> ProfileEdit(UserViewModel data)
        {
            if (ModelState.IsValid)
            {
                var userId = User.GetId();
                var user = _dbContext.Users.FirstOrDefault(u => u.Id == userId);
                if (user == null)
                    return RedirectToAction("Index", "Home");

                user.FirstName = data.FirstName;
                user.Surname = data.Surname;

                await _dbContext.SaveChangesAsync();

                return Redirect("~/profile");
            }

            return RedirectToAction("Profile");
        }
    }
}