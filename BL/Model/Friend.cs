using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Model
{
    public class Friend
    {
        public int MainId { get; set; }
        public User Main { get; set; } = null!;
        public int UserFriendId { get; set; }
        public User UserFriend { get; set; } = null!;
        public bool IsSuccess { get; set; }
    }
}
