using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AirIQ.Models.Request;
using AirIQ.Models.Response;
using Refit;

namespace AirIQ.Services.Interfaces
{
    public interface IZoopApi
    {
        [Post("/api/v1/in/identity/pan/lite")]
        Task<ZoopApiResponse<PanValidationDto>> ValidatePanAsync([Body] PanLiteRequest request);

        [Post("/api/v1/in/merchant/gstin/lite")]
        Task<ZoopApiResponse<GstValidationDto>> ValidateGstAsync([Body] GstLiteRequest request);
    }
}