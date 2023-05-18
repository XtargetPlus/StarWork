using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Model
{
    public class UsersChats
    {
        public int UserId { get; set; }
        public User User { get; set; } = null!; 
        public int ChatId { get; set; }
        public Chat Chat { get; set; } = null!;
        public int RoleId { get; set; }
        public Role Role { get; set; } = null!;
        public byte[] Key { get; set; }
        public bool IsSuccess { get; set; }
    }
}
