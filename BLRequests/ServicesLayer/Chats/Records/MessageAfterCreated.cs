using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLRequests.ServicesLayer.Chats.Records
{
    public class MessageAfterCreated
    {
        public int MessageId { get; set; }
        public string Text { get; set; } = default!;
        public string NickName { get; set; } = default!;
        public string DateTime { get; set; } = default!;
    }
}
