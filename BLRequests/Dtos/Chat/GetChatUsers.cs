using BLRequests.Dtos.Friend;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLRequests.Dtos.Chat
{
    public class GetChatUsers : GetAllFriendsDto
    {
        public int Role { get; set; }
    }
}
