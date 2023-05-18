using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLRequests.Dtos.Profile
{
    public class MyProfileInfoDto : ProfileInfoDto
    {
        public string Phone { get; set; } = null!;
    }
}
