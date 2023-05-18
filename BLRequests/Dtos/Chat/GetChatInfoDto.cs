using BLRequests.Dtos.Friend;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLRequests.Dtos.Chat
{
    public class GetChatInfoDto
    {
        public int ChatId { get; set; }
        public int UserId { get; set; }
        public string ChatName { get; set; } = default!;
        public string? Note { get; set; }
        public List<GetChatUsers> UsersInChat { get; set; } = new();
    }
}
