﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLRequests.Dtos
{
    public class EncryptDataDto
    {
        public string Login { get; set; } = default!;
        public string Password { get; set; } = default!;
    }
}
