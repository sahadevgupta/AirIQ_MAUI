using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AirIQ.Models.Response
{
    public record SignupDto
    {
        public string? Message { get; set; }
        public int AccoundId { get; set; }
    }
}