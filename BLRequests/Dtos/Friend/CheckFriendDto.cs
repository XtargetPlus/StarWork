using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLRequests.Dtos.Friend
{
    public class CheckFriendDto
    {
        public int Id { get; set; }
        public string NickName { get; set; } = default!;
    }
}
