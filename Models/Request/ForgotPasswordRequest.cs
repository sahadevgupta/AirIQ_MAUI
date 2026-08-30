using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AirIQ.Models.Request
{
    public class ForgotPasswordRequest
    {
        public string EmailOrPhone { get; set; } = string.Empty;
    }
}