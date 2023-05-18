using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLRequests.Dtos.Chat
{
    public class SendMessageDto
    {
        public int ChatId { get; set; }
        public int? UserId { get; set; }
        public string Message { get; set; } = default!;
        public string? Password { get; set; }
    }
}
