using BLRequests.Dtos;
using BLRequests.Dtos.Profile;
using BLRequests.Repositories.Profile;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StarWork3.SubFunctions;
using System.ComponentModel;
using System.Security.Claims;

namespace StarWork3.Controllers
{
    [Authorize]
    [Route("/profile")]
    public class ProfileController : Controller
    {
        public readonly IProfileService _profileService;

        public ProfileController(IProfileService profileService)
        {
            _profileService = profileService;
        }

        [HttpGet]
        [Authorize, Route("/profile/get-my-info")]
        public async Task<IActionResult> GetMyProfileInfo()
        {
            if (int.TryParse(HttpContext.User.FindFirstValue("UserId"), out int id))
            {
                MyProfileInfoDto? profileInfo = await _profileService.GetMyInfoAsync(id);
                if (profileInfo == null)
                    return BadRequest();
                profileInfo.Phone = HttpContext.User.FindFirstValue(ClaimTypes.MobilePhone) ?? "";
                profileInfo.Email = HttpContext.User.FindFirstValue(ClaimTypes.Email) ?? "";
                _ = int.TryParse(Request.Cookies["age"], out int age);
                profileInfo.Age = age;
                profileInfo.NickName = HttpContext.User.FindFirstValue("UserName") ?? "";
                return Json(profileInfo);
            }
            return BadRequest();
        }

        [HttpGet]
        [Authorize, Route("/profile/get-info")]
        public async Task<IActionResult> GetProfileInfo(string nick)
        {
            return Json(await _profileService.GetInfoAsync(nick));
        }

        [HttpPost]
        [Authorize, Route("/profile/update-info")]
        public async Task<IActionResult> UpdateProfileInfo([FromBody] MyProfileInfoDto profileInfo)
        {
            if (int.TryParse(HttpContext.User.FindFirstValue("UserId"), out int id))
            {
                string phone = HttpContext.User.FindFirstValue(ClaimTypes.MobilePhone) ?? "";
                EncryptDataDto encryptData = null!;
                if (phone != profileInfo.Phone)
                {
                    encryptData = new()
                    {
                        Login = HttpContext.User.FindFirstValue(ClaimTypes.Name) ?? "",
                        Password = HttpContext.User.FindFirstValue("Password") ?? "",
                    };
                }
                int? result = await _profileService.UpdateInfoAsync(id, profileInfo, encryptData, phone);
                if (result == null)
                    return BadRequest();
                await HttpContext.ChangeBaseUserClaimsAsync(profileInfo);
                Response.ChangeCookiesAsync(Request, profileInfo);
                return Ok();
            }
            return BadRequest();
        }

        [HttpPost]
        [Authorize, Route("/profile/update-login")]
        public async Task<IActionResult> UpdateLogin([FromBody] ChangingLoginDto changingLogin)
        {
            if (changingLogin.OldLogin != HttpContext.User.FindFirstValue(ClaimTypes.Name))
                return BadRequest();
            if (changingLogin.NewLogin == HttpContext.User.FindFirstValue(ClaimTypes.Name))
                return Ok();
            if (int.TryParse(HttpContext.User.FindFirstValue("UserId"), out int id))
            {
                int? result = await _profileService.ChangeLoginAsync(id, changingLogin.NewLogin);
                if (result == null)
                    return BadRequest();
                await HttpContext.ChangeLoginClaimAsync(changingLogin.NewLogin);
            }
            return Ok();
        }

        [HttpPost]
        [Authorize, Route("/profile/update-password")]
        public async Task<IActionResult> UpdatePassword([FromBody] ChangingPasswordDto changingPassword)
        {
            if (changingPassword.OldPassword != HttpContext.User.FindFirstValue("Password"))
                return BadRequest();
            if (changingPassword.NewPassword == HttpContext.User.FindFirstValue("Password"))
                return Ok();
            if (int.TryParse(HttpContext.User.FindFirstValue("UserId"), out int id))
            {
                int? result = await _profileService.ChangePasswordAsync(id, changingPassword);
                if (result == null)
                    return BadRequest();
                await HttpContext.ChangePasswordClaimAsync(changingPassword.NewPassword);
            }
            return Ok();
        }
    }
}
