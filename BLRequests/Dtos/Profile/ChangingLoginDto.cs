using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLRequests.Dtos.Profile
{
    public class ChangingLoginDto
    {
        public string OldLogin { get; set; } = default!;
        public string NewLogin { get; set; } = default!;
    }
}
