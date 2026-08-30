using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AirIQ.Models.Request
{
    public class ChangePasswordRequest
    {
        public int AccountId { get; set; }
        public string? OldPassword { get; set; }
        public string? NewPassword { get; set; }
    }
}