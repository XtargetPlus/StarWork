using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLRequests.Dtos.Message
{
    public class GetLastMessageToChatsListDto
    {
        public byte[] EncryptMessage { get; set; } = default!;
        public DateTime DateTime { get; set; }
    }
}
