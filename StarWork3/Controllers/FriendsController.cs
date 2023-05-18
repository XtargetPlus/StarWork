using BLRequests.Repositories.Profile;
using BLRequests.ServicesLayer.Friends;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace StarWork3.Controllers
{
    [Authorize]
    [Route("/friends")]
    public class FriendsController : Controller
    {
        public readonly IFriendsService _friendsService;

        public FriendsController(IFriendsService friendsService)
        {
            _friendsService = friendsService;
        }

        [HttpGet]
        [Authorize, Route("/friends/get-friends")]
        public async Task<IActionResult> GetAllFriends()
        {
            return Json(await _friendsService.GetAllFriendsAsync(int.Parse(HttpContext.User.FindFirstValue("UserId") ?? "0")));
        }

        [HttpGet]
        [Authorize, Route("/friends/get-friends-with-id")]
        public async Task<IActionResult> GetAllFriendsWithId()
        {
            return Json(await _friendsService.GetAllFriendsWithIdAsync(int.Parse(HttpContext.User.FindFirstValue("UserId") ?? "0")));
        }

        [HttpGet]
        [Authorize, Route("/friends/get-users")]
        public async Task<IActionResult> GetAllUsers()
        {
            return Json(await _friendsService.GetAllUsersAsync(int.Parse(HttpContext.User.FindFirstValue("UserId") ?? "0")));
        }

        [HttpGet]
        [Authorize, Route("/friends/get-applications")]
        public async Task<IActionResult> GetAllApplications()
        {
            return Json(await _friendsService.GetAllApplicationsAsync(int.Parse(HttpContext.User.FindFirstValue("UserId") ?? "0")));
        }

        [HttpPost]
        [Authorize, Route("/friends/add")]
        public async Task<IActionResult> Add([FromBody] string nick)
        {
            int? result = await _friendsService.AddAsync(nick, int.Parse(HttpContext.User.FindFirstValue("UserId") ?? "0"));
            if (result == null)
                return BadRequest();
            return Ok();
        }

        [HttpPost]
        [Authorize, Route("/friends/accept")]
        public async Task<IActionResult> Accept([FromBody] string nick)
        {
            int? result = await _friendsService.AcceptAsync(nick, int.Parse(HttpContext.User.FindFirstValue("UserId") ?? "0"));
            if (result == null)
                return BadRequest();
            return Ok();
        }

        [HttpPost]
        [Authorize, Route("/friends/cancel")]
        public async Task<IActionResult> Cancel([FromBody] string nick)
        {
            int? result = await _friendsService.CancelAsync(nick, int.Parse(HttpContext.User.FindFirstValue("UserId") ?? "0"));
            if (result == null)
                return BadRequest();
            return Ok();
        }

        [HttpDelete]
        [Authorize, Route("/friends/delete")]
        public async Task<IActionResult> DeleteFriend([FromBody] string nick)
        {
            int? result = await _friendsService.DeleteAsync(nick, int.Parse(HttpContext.User.FindFirstValue("UserId") ?? "0"));
            if (result == null)
                return BadRequest();
            return Ok();
        }
    }
}
