using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using sem2_FSharp;
using sem2.Models;
using sem2.Views;

namespace sem2.Controllers
{
    public class ApiController: Controller
    {
        private readonly ApplicationContext _context;

        public ApiController(ApplicationContext context)
        {
            this._context = context;
        }

        [Route("~/api/get_film_info")]
        public IActionResult GetFilmInfo([FromQuery(Name = "filmId")] int filmId = -1)
        {
            var film = _context.Films.SingleOrDefault(f => f.Id == filmId);
            if (film == null)
                return BadRequest("{}");
            return Ok(JsonConvert.SerializeObject(FilmHelpers.FromFilm(film)));
        }
    }
}