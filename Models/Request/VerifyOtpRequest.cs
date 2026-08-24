using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AirIQ.Models.Request
{
    public class VerifyOtpRequest
    {
        public string? TransactionKey { get; set; }
        public string? OTP { get; set; }
    }
}