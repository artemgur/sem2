using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using sem2_FSharp;
using sem2.Models;

namespace sem2.Controllers
{
    public class ApiController: Controller
    {
        private readonly ApplicationContext context;

        public ApiController(ApplicationContext context)
        {
            this.context = context;
        }

        [Route("~/api/get_film_info")]
        public string GetFilmInfo([FromQuery(Name = "filmId")] int filmId = -1)
        {
            var film = context.Films.ById(filmId).SingleOrDefault();
            if (film == null)
            {
                HttpContext.Response.StatusCode = 400;
                return "{}";
            }
            return JsonConvert.SerializeObject(FilmDTO.FromFilm(film));
        }
    }
}