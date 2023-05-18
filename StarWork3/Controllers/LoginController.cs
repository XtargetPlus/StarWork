using BL;
using BL.Model;
using BLRequests.AuthRequests;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Text;

namespace StarWork3.Controllers
{
    [Route("/login")]
    public class LoginController : Controller
    {
        private DbApplicationContext _db { get; } = null!;

        public LoginController(DbApplicationContext db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<IActionResult> Login()
        {
            Response.ContentType = "text/html; charset=utf-8";
            await Response.SendFileAsync("wwwroot/Login_v8/login.html");
            return Ok();
        }
        [HttpPost, Route("/login/get-user")]
        public async Task<IActionResult> GetUser([FromBody] AuthValue authValue)
        {
            LoginRequest loginRequest = new(_db);

            AuthInfo? userInfo = await loginRequest.GetUser(authValue.Login, authValue.Password);
            loginRequest.Dispose();
            if (userInfo != null)
            {
                List<Claim> claims = new()
                {
                    new Claim(ClaimTypes.Name, userInfo.Login),
                    new Claim("UserName", userInfo.User!.NickName),
                    new Claim("Password", authValue.Password),
                    new Claim("IV", string.Join(',', userInfo.IV)),
                    new Claim(ClaimTypes.Email, userInfo!.User!.Email),
                    new Claim(ClaimTypes.MobilePhone, Encoding.UTF8.GetString(userInfo.User!.Phone)),
                    new Claim("UserId", userInfo.User.Id.ToString())
                };
                Response.Cookies.Append("age", userInfo.User!.Age.ToString());
                Response.Cookies.Append("email", userInfo.User!.Email);
                Response.Cookies.Append("phone", Encoding.UTF8.GetString(userInfo.User!.Phone));

                ClaimsIdentity claimsIdentity = new(claims, "Cookies");
                ClaimsPrincipal claimsPrincipal = new(claimsIdentity);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal);
                return Ok();
            }
            return BadRequest();
        }
    }
    public record class AuthValue(string Login, string Password);
}
