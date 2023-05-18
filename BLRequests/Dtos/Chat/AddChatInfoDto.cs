using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLRequests.Dtos.Chat
{
    public class AddChatInfoDto
    {
        public string Title { get; set; } = null!;
        public string? Note { get; set; }
        public List<int> UsersId { get; set; } = new();
    }
}
