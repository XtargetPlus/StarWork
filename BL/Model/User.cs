using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Model
{
    public class User
    {
        public int Id { get; set; }
        public string NickName { get; set; } = null!;
        public int Age { get; set; }
        public byte[] Phone { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? Note { get; set; }
        public DateTime? LastEntrance { get; set; }
        public List<AuthInfo> AuthInfos { get; set; } = null!;
        public List<UsersChats> UsersChats { get; set; } = null!;
        public List<Message> Messages { get; set; } = null!;
        public List<Friend> Friends { get; set; } = null!;
        public List<Friend> Mains { get; set; } = null!;
    }
}
