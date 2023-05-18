using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Model
{
    public class Chat
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string? Note { get; set; }
        public DateTime Created { get; set; }
        public byte[] Key { get; set; } = null!;
        public byte[] IV { get; set; } = null!;
        public byte[] Salt { get; set; } = null!;
        public List<UsersChats> UsersChats { get; set; } = new();
        public List<Message> Messages { get; set; } = new();
    }
}
