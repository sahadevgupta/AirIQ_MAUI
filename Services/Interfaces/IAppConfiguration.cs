using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AirIQ.Services.Interfaces
{
    public interface IAppConfiguration
    {
        string BaseUrl { get; }
        string ApiKey { get; }
        string ZoopApiKey { get; }
        string ZoopAppId { get; }
        string ForgotPasswordPolicy { get; }
        string AuthorityBase { get; }
        string PortalUrl { get; }
        string PrivacyUrl { get; }
        string MicrosoftClarityProjectId { get; }
    }
}