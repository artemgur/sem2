using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using sem2.Models;

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
            var films = context.Films.Select(x => x.Id);//TODO DTO?
            return View(films);
        }
        
        public IActionResult AboutFilm(int id)
        {
            var film = context.Films.Single(x => x.Id == id);
            var dto = new FilmDTO
            {
                Id = film.Id,
                Info = film.Info,
                Name = film.Name,
                Producer = film.Producer,
                LongDescription = film.LongDescription,
                OriginalName = film.OriginalName,
                ShortDescription = film.ShortDescription,
                Actors = film.Actors.Split(',')
            };
            return View("AboutFilm", dto);
        }    
    }
}