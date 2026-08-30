using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AirIQ.Models.Request
{
    public class ResetPasswordRequest
    {
        public string? TransactionKey { get; set; }
        public string? OTP { get; set; }
        public string? NewPassword { get; set; }
    }
}