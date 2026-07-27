using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AirIQ.Models.Request;
using AirIQ.Models.Response;
using AirIQ.Services.Interfaces;

namespace AirIQ.Services
{
    public class ZoopVerificationService(IZoopApi zoopApi) : IZoopVerificationService
    {
        public async Task<PanValidationDto> ValidatePanAsync(string panNumber, string holderName)
        {
            var response = await zoopApi.ValidatePanAsync(new PanLiteRequest
            {
                PanData = new PanDataRequest
                {
                    CustomerPanNumber = panNumber,
                    PanHolderName = holderName
                }
            });

            return response.Result ?? null!;
        }

        public async Task<GstValidationDto> ValidateGstAsync(string gstNumber)
        {
            var response = await zoopApi.ValidateGstAsync(new GstLiteRequest
            {
                GstData = new GstDataRequest
                {
                    BusinessGstinNumber = gstNumber
                }
            });

            return response.Result ?? null!;
        }
    }
}