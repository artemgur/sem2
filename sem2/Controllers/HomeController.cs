using System.Diagnostics;
using DomainModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using sem2.Models;
using sem2.Models.ViewModels;
using sem2_FSharp.ViewModels;

namespace sem2.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, ApplicationContext dbContext)
        {
            _logger = logger;
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
    }
}