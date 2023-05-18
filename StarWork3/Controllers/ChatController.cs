using BLRequests.Dtos.Chat;
using BLRequests.ServicesLayer.Chats;
using BLRequests.ServicesLayer.Chats.Records;
using BLRequests.ServicesLayer.Friends;
using BLRequests.ServicesLayer.Friends.Concrete;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace StarWork3.Controllers
{
    [Authorize]
    [Route("/chats")]
    public class ChatController : Controller
    {
        public readonly IChatService _chatsService;

        public ChatController(IChatService chatsService)
        {
            _chatsService = chatsService;
        }

        [HttpGet]
        [Authorize, Route("/chats/get-all")]
        public async Task<IActionResult> GetAll()
        {
            if (int.TryParse(HttpContext.User.FindFirstValue("UserId"), out int id))
            {
                return Json(await _chatsService.GetAllAsync(id,
                    HttpContext.User.FindFirstValue("Password") ?? throw new ArgumentNullException("Пароль отсутствует")));
            }
            return BadRequest();
        }

        [HttpGet]
        [Authorize, Route("/chats/get-all-messages")]
        public async Task<IActionResult> GetAllMessages([FromQuery] int chatId)
        {
            if (int.TryParse(HttpContext.User.FindFirstValue("UserId"), out int id))
            {
                return Json(await _chatsService.GetAllMessagesAsync(chatId,
                    id,
                    HttpContext.User.FindFirstValue("Password") ?? throw new ArgumentNullException("Пароль отсутствует")));
            }
            return BadRequest();
        }

        [HttpPost]
        [Authorize, Route("/chats/add")]
        public async Task<IActionResult> Add([FromBody] AddChatInfoDto chatInfoDto)
        {
            if (int.TryParse(HttpContext.User.FindFirstValue("UserId"), out int id))
            {
                int? chatId = await _chatsService.CreateAsync(chatInfoDto, 
                    id, 
                    HttpContext.User.FindFirstValue("Password") ?? throw new ArgumentNullException("Пароль отсутствует"));
                if (chatId == null)
                    return BadRequest();
                return Json(chatId);
            }
            return BadRequest();
        }

        [HttpPost]
        [Authorize, Route("/chats/send-message")]
        public async Task<IActionResult> SendMessage([FromBody] SendMessageDto messageDto)
        {
            if (int.TryParse(HttpContext.User.FindFirstValue("UserId"), out int id))
            {
                messageDto.UserId = id;
                messageDto.Password = HttpContext.User.FindFirstValue("Password") ?? throw new ArgumentNullException("Пароль отсутствует");
                MessageAfterCreated? messageAfterCreated = await _chatsService.SendMessageAsync(messageDto);
                if (messageAfterCreated == null)
                    return BadRequest();
                messageAfterCreated.NickName = HttpContext.User.FindFirstValue("UserName") ?? throw new ArgumentNullException("Ник отсутствует");
                return Json(messageAfterCreated);
            }
            return BadRequest();
        }

        [HttpPost]
        [Authorize, Route("/chats/update-info")]
        public async Task<IActionResult> Update([FromBody] UpdateChatInfoDto infoDto)
        {
            int? result = await _chatsService.UpdateInfoAsync(infoDto);
            if (result == null)
                return BadRequest();
            return Ok();
        }
        

        [HttpGet]
        [Authorize, Route("/chats/get-info")]
        public async Task<IActionResult> GetInfo([FromQuery] int chatId)
        {
            if (int.TryParse(HttpContext.User.FindFirstValue("UserId"), out int id))
            {
                GetChatInfoDto? chatInfo = await _chatsService.GetMoreInfoAsync(chatId, id);
                if (chatInfo == null)
                    return BadRequest();
                return Json(chatInfo);
            }
            return BadRequest();
        }

        [HttpDelete]
        [Authorize, Route("/chats/expel-user")]
        public async Task<IActionResult> ExpelUser([FromBody] ExpelUserDto userDto)
        {
            int? result = await _chatsService.ExpelUserAsync(userDto);
            if (result == null)
                return BadRequest();
            return Ok();
        }
    }
}
