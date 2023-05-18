using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLRequests.Dtos.Message
{
    public class GetAllMessagesDto
    {
        public int Id { get; set; }
        public string NickName { get; set; } = default!;
        public string Message { get; set; } = default!;
        public string PostingTime { get; set; } = default!;
        public bool IsHost { get; set; }
    }
}
