using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AirIQ.Models.Response
{
    public record ForgotPasswordDto
    {
        public string? Message { get; set; }
        public string? TransactionKey { get; set; }
        public string? EmailDelivery { get; set; }
        public string? SmsDelivery { get; set; }
    }
}