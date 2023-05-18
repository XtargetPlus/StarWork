using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Model
{
    public class MessageFiles
    {
        public int Id { get; set; }
        public string File { get; set; } = null!;
        public int MessageId { get; set; }
        public Message? Message { get; set; }
    }
}
