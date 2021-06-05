using System.Linq;
using System.Threading.Tasks;
using System.Linq;
using Authentication.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using sem2.Infrastructure.Services;
using sem2_FSharp;
using sem2.Models;
using sem2.Models.ViewModels.HomeModels;
using sem2.Views;

namespace sem2.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationContext _context;
        private readonly PermissionService _permissionService;

        public HomeController(ILogger<HomeController> logger, ApplicationContext dbContext, PermissionService permissionService)
        {
            _logger = logger;
            _context = dbContext;
            _permissionService = permissionService;
        }

        [Route("~/")]
        [Route("~/Home")]
        public async Task<IActionResult> Index()
        {
            var hasMediaPlus = await _permissionService.HasSubscriptionPlan(3);
            return View(hasMediaPlus);
        }
        //
        // public IActionResult Privacy()
        // {
        //     return View();
        // }
        public IActionResult Catalog([FromQuery (Name = "query")] string query)
        {
            var films = _context.Films
                .Select(x => FilmHelpers.FromFilm(x));

            if (!string.IsNullOrWhiteSpace(query))
                films = films.Where(x => x.Name.ToLower().Contains(query.ToLower()));

            var model = new CatalogViewModel()
            {
                Films = films.ToList(),
                Query = query
            };
            
            return View(model);
        }

        public IActionResult Favorite()
        {
            var userId = User.GetId();

            var films = _context.Users.Where(u => u.Id == userId)
                .SelectMany(user => user.FavoriteFilms)
                .Select(x => FilmHelpers.FromFilm(x))
                .ToList();

            var model = new CatalogViewModel()
            {
                Films = films
            };
            
            return View("Catalog", model);

        }
        
        [Route("~/AboutFilm/{id:int}")]
        public IActionResult AboutFilm(int id)
        {
            var userId = User.GetId();
            var isInFavorites = _context.Users
                .Where(user => user.Id == userId)
                .SelectMany(user => user.FavoriteFilms)
                .Any(f => f.Id == id);
            
            var film = _context.Films.SingleOrDefault(f => f.Id == id);
            if (film == null)
                return RedirectToAction("Catalog");

            var dto = FilmHelpers.FromFilm(film, isInFavorites);

            return View("AboutFilm", dto);
        }

        [HttpGet("~/film/{filmId:int}/watch")]
        [Authorize]
        public async Task<IActionResult> WatchFilm(int filmId)
        {
            var hasPermission = await _permissionService.HasAllPermissions("WatchPermission");
            if (!hasPermission)
                return Redirect(Url.Content($"~/AboutFilm/{filmId}"));

            var film = _context.Films.FirstOrDefault(f => f.Id == filmId);
            if (film == null)
                return RedirectToAction("Catalog");

            var model = new WatchFilmModel()
            {
                FilmId = film.Id,
                VideoPath = film.VideoPath
            };
            
            return View(model);
        }
        
        // [HttpGet("~/filmLogos/{userId}/image")]
        // public IActionResult GetLogo(int filmId)
        // {
        //     var data = context.Films.LogoById(filmId).FirstOrDefault();
        //     if (data == null)
        //         return BadRequest();
        //
        //     return File(data.ImagePath, data.ContentType);
        // }
        //
        // [HttpGet("~/filmBackgrounds/{userId}/image")]
        // public IActionResult GetBackground(int filmId)
        // {
        //     var data = context.Films.BackgroundById(filmId).FirstOrDefault();
        //     if (data == null)
        //         return BadRequest();
        //
        //     return File(data.ImagePath, data.ContentType);
        // }
        
        [Route("~/AddToFavorite")]
        [HttpPost]
        public IActionResult AddToFavorite([FromQuery (Name = "filmId")] int filmId = -1)
        {
            var userId = User.GetId();
            var user = _context.Users.SingleOrDefault(u => u.Id == userId);
            var film = _context.Films.SingleOrDefault(f => f.Id == filmId);
            if (film == null || user == null)
                return BadRequest();
            _context.Entry(user).Collection(u => u.FavoriteFilms).Load();
            user.FavoriteFilms.Add(film);
            //film.InFavoritesOfUsers.Add(user);
            _context.SaveChanges();
            return Ok();
        }
        [Route("~/RemoveFromFavorite")]
        [HttpPost]
        public IActionResult RemoveFromFavorite([FromQuery (Name = "filmId")] int filmId = -1)
        {
            var userId = User.GetId();
            var user = _context.Users.SingleOrDefault(u => u.Id == userId);
            var film = _context.Films.SingleOrDefault(f => f.Id == filmId);
            if (film == null || user == null)
                return BadRequest();
            _context.Entry(user).Collection(u => u.FavoriteFilms).Load();
            var a = user.FavoriteFilms;
            user.FavoriteFilms.Remove(film);
            //film.InFavoritesOfUsers.Remove(user);
            _context.SaveChanges();
            return Ok();
        }
    }
}