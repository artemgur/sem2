﻿using System.Linq;
using Authentication.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using sem2_FSharp;
using sem2.Models;
using sem2.Views;

namespace sem2.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationContext dbContext)
        {
            _logger = logger;
            _context = dbContext;
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
            var films = _context.Films
                .Select(x => FilmHelpers.FromFilm(x))
                .ToList();//TODO DTO?
            return View(films);
        }

        public IActionResult Favorite()
        {
            var userId = User.GetId();
            var user = _context.Users.FirstOrDefault(u => u.Id == userId);
            if (user == null)
                return RedirectToAction("Index", "Home");
            var films = user.FavoriteFilms.Select(x => FilmHelpers.FromFilm(x));
            return View("Catalog", films);
        }
        
        [Route("~/AboutFilm/{id:int}")]
        public IActionResult AboutFilm(int id)
        {
            var film = _context.Films.SingleOrDefault(f => f.Id == id);
            if (film == null)
                return RedirectToAction("Catalog");
            var dto = FilmHelpers.FromFilm(film);
            return View("AboutFilm", dto);
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
        
        [Route("~/add_to_favorite")]
        public void AddToFavorite([FromQuery (Name = "filmId")] int filmId = -1)
        {
            if (filmId == -1)
                return;
            var userId = User.GetId();
            var user = _context.Users.FirstOrDefault(u => u.Id == userId);
            if (user == null)
                return;
            var film = _context.Films.SingleOrDefault(f => f.Id == filmId);
            if (film == null)
                return;
            user.FavoriteFilms.Add(film);
            _context.SaveChanges();
        }

        public IActionResult Search(string query)
        {
            query = query.ToLower();
            var films = _context.Films
                .Where(x => x.Name.ToLower().Contains(query))
                .Select(x => FilmHelpers.FromFilm(x))
                .ToList();
            return View("Catalog", films);
        }
    }
}