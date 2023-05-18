using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Model
{
    public class Message
    {
        public int Id { get; set; }
        public byte[] Text { get; set; } = null!;
        public bool IsRead { get; set; }
        public DateTime DateTime { get; set; }
        public int? FatherId { get; set; }
        public Message? Father { get; set; }
        public int? ChildId { get; set; }
        public Message? Child { get; set; }
        public int UserId { get; set; }
        public User? User { get; set; }
        public int ChatId { get; set; }
        public Chat? Chat { get; set; }
        public List<MessageFiles>? MessageFiles { get; set; }
    }
}
