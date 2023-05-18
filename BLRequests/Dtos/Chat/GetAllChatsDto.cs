using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLRequests.Dtos.Chat
{
    public class GetAllChatsDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = default!;
        [DefaultValue("")]
        public string LastMessage { get; set; } = default!;
        [DefaultValue("")]
        public string PostedTime { get; set; } = default!;
    }
}
