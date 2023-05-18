using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLRequests.Dtos.Chat
{
    public class UpdateChatInfoDto
    {
        public int ChatId { get; set; }
        public string ChatName { get; set; } = default!;
        public string Note { get; set; } = default!;
    }
}
