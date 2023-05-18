using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLRequests.Dtos.Profile
{
    public class ProfileInfoDto
    {
        public string NickName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public int Age { get; set; } = default!;
        public string Note { get; set; } = null!;
    }
}
