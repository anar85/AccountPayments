﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentsApplication.Models.Configs
{
    public class TokenSettings
    {
        public string? Secret { get; set; }
        public int TtlAccessToken { get; set; }
    }
}
