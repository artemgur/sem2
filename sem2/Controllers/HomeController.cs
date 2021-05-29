using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace sem2.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationContext context;

        public HomeController(ILogger<HomeController> logger, ApplicationContext dbContext)
        {
            _logger = logger;
            context = dbContext;
        }

        public IActionResult Index()
        {
            return View();
        }
        //
        // public IActionResult Privacy()
        // {
        //     return View();
        // }
        public IActionResult Catalog()
        {
            return View();
        }
        
        public IActionResult AboutFilm()
        {
            return View("AboutFilm");
        }    
    }
}