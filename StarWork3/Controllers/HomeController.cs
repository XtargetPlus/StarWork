using BL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace StarWork3.Controllers
{
    [Authorize]
    [Route("/")]
    public class HomeController : Controller
    {
        private DbApplicationContext _db { get; } = null!;
        public HomeController(DbApplicationContext db)
        {
            _db = db;
        }

        [HttpGet]
        [Authorize]
        public async Task Home()
        {
            Response.ContentType = "text/html; charset=utf-8";
            await Response.SendFileAsync("wwwroot/main.html");
        }
    }
}
