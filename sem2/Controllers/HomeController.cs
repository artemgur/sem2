using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using sem2_FSharp;
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

        public IActionResult Favorite()
        {
            var userId = User.GetId();
            var user = context.Users.ById(userId).FirstOrDefault();
            if (user == null)
                return RedirectToAction("Index", "Home");
            var films = user.FavoriteFilms.Select(x => x.Id);
            return View("Catalog", films);
        }
        
        public IActionResult AboutFilm(int id)
        {
            var film = context.Films.Single(x => x.Id == id);
            var dto = FilmDTO.FromFilm(film);
            return View("AboutFilm", dto);
        }
        
        [HttpGet("~/filmLogos/{userId}/image")]
        public IActionResult GetLogo(int filmId)
        {
            var data = context.Films.LogoById(filmId).FirstOrDefault();
            if (data == null)
                return BadRequest();

            return File(data.ImagePath, data.ContentType);
        }
        
        [HttpGet("~/filmBackgrounds/{userId}/image")]
        public IActionResult GetBackground(int filmId)
        {
            var data = context.Films.BackgroundById(filmId).FirstOrDefault();
            if (data == null)
                return BadRequest();

            return File(data.ImagePath, data.ContentType);
        }
        
        [Route("~/add_to_favorite")]
        public void AddToFavorite([FromQuery (Name = "filmId")] int filmId = -1)
        {
            if (filmId == -1)
                return;
            var userId = User.GetId();
            var user = context.Users.ById(userId).FirstOrDefault();
            if (user == null)
                return;
            var film = context.Films.ById(filmId).SingleOrDefault();
            if (film == null)
                return;
            user.FavoriteFilms.Add(film);
            context.SaveChanges();
        }
    }
}