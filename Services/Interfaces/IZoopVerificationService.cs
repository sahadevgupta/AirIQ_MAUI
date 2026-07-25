using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AirIQ.Models.Response;

namespace AirIQ.Services.Interfaces
{
    public interface IZoopVerificationService
    {
        Task<PanValidationDto> ValidatePanAsync(string panNumber, string holderName);
        Task<GstValidationDto> ValidateGstAsync(string gstNumber);
    }
}