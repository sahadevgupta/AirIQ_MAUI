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
            try
            {
                var response = await zoopApi.ValidatePanAsync(new PanLiteRequest
                {
                    PanData = new PanDataRequest
                    {
                        CustomerPanNumber = panNumber,
                        PanHolderName = holderName
                    }
                });

                if (!response.Success)
                {
                    throw new InvalidOperationException(response.ResponseMessage ?? "PAN validation failed.");
                }

                return response.Result ?? null!;
            }
            catch (Exception exception)
            {
                SentrySdk.CaptureException(exception);
                throw;
            }
        }

        public async Task<GstValidationDto> ValidateGstAsync(string gstNumber)
        {
            try
            {
                var response = await zoopApi.ValidateGstAsync(new GstLiteRequest
                {
                    GstData = new GstDataRequest
                    {
                        BusinessGstinNumber = gstNumber
                    }
                });

                if (!response.Success)
                {
                    throw new InvalidOperationException(response.ResponseMessage ?? "GST validation failed.");
                }

                return response.Result ?? null!;
            }
            catch (Exception exception)
            {
                SentrySdk.CaptureException(exception);
                throw;
            }
        }
    }
}