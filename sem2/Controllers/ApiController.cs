using System.Linq;
using System.Threading.Tasks;
using Authentication.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using sem2.Infrastructure.Services;
using sem2_FSharp;
using sem2.Models;
using sem2.Views;

namespace sem2.Controllers
{
    [ApiController]
    public class ApiController: Controller
    {
        private readonly ApplicationContext _context;
        private readonly PermissionService _permissionService;

        public ApiController(ApplicationContext context, PermissionService permissionService)
        {
            this._context = context;
            _permissionService = permissionService;
        }

        [Route("~/api/get_film_info")]
        public IActionResult GetFilmInfo([FromQuery(Name = "filmId")] int filmId = -1)
        {
            var film = _context.Films.SingleOrDefault(f => f.Id == filmId);
            var userId = User.GetId();
            var isInFavorites = _context.Users
                .Where(user => user.Id == userId)
                .SelectMany(user => user.FavoriteFilms)
                .Any(f => f.Id == filmId);
            if (film == null)
                return BadRequest("{}");
            return Ok(JsonConvert.SerializeObject(FilmHelpers.FromFilm(film, isInFavorites)));
        }

        [HttpGet("~/api/hasWatchPermission")]
        public async Task<IActionResult> HasWatchPermission()
        {
            var hasWatchPermission = await _permissionService.HasAllPermissions("WatchPermission");
            return Ok(hasWatchPermission);
        }
    }
}