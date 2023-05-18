using BL;
using BLRequests.AuthRequests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace StarWork3.Controllers
{
    [Route("/registr")]
    public class RegistrationController : Controller
    {
        private DbApplicationContext _db { get; } = null!;
        public RegistrationController(DbApplicationContext db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<IActionResult> Registraion()
        {
            Response.ContentType = "text/html; charset=utf-8";
            await Response.SendFileAsync("wwwroot/Registration/registration.html");
            return Ok();
        }

        [HttpPost, Route("/registr/add-user")]
        public async Task<IActionResult> AddUser([FromBody] RegistrValues registrValues)
        {
            RegistrRequests registr = new(_db);

            var result = await registr.AddUser(
                registrValues.FullName, registrValues.Age, registrValues.MobileNumber,
                registrValues.Email, registrValues.Password,
                registrValues.Login);
            registr.Dispose();
            if (result == 1)
            {
                return Ok();
            }
            return BadRequest();
        }
    }
    public record class RegistrValues(string FullName, int Age, string MobileNumber, string Email, string Login, string Password);
}
