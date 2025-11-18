using AirIQ.Configurations.CustomExceptions;
using AirIQ.Constants;
using AirIQ.Services.Interfaces;
using Refit;
using System.Diagnostics;
using System.Text;

namespace AirIQ.Extensions
{
    public static class RefitClientServiceInitializer
    {
        public static MauiAppBuilder RefitClientInit(this MauiAppBuilder builder)
        {
            builder.Services.AddTransient<HttpMessageLogHandler>();
            Uri defaultUri = new Uri(AppConfiguration.BaseUrl);

            builder.Services.AddRefitClient<IAppBackendService>(new RefitSettings
            {
                ExceptionFactory = async (response) =>
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        return new UnauthorizedException($"Unauthorized: {content}");
                    }
                    return null; // Default for other status codes            
                }
            })
            .ConfigureHttpClient(j =>
            {
                j.BaseAddress = defaultUri;
            })
#if DEBUG
            .AddHttpMessageHandler<HttpMessageLogHandler>()
#endif
            ;

            return builder;
        }
    }

    public class HttpMessageLogHandler : DelegatingHandler
    {
        private readonly string BaseAddress = AppConfiguration.BaseUrl;

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var relativeUrl = request.RequestUri!.AbsoluteUri.Substring(BaseAddress.Length);
            var newUri = Combine(BaseAddress, relativeUrl);
            request.RequestUri = new Uri(newUri);
            var req = request;
            var id = Guid.NewGuid().ToString();
            var msg = $"[{id} -   Request]";
            StringBuilder apiDetails = new();



            Debug.WriteLine($"{msg}========Start==========");
            Debug.WriteLine($"{msg} {req.Method} {req.RequestUri.PathAndQuery} {req.RequestUri.Scheme}/{req.Version}");
            Debug.WriteLine($"{msg} Host: {req.RequestUri.Scheme}://{req.RequestUri.Host}");

            apiDetails.Append($"{"Starting Api Call "}{req.Method}");
            apiDetails.Append($"{"RequestUri "}{req.RequestUri.PathAndQuery}");

            foreach (var header in req.Headers)
                Debug.WriteLine($"{msg} {header.Key}: {string.Join(", ", header.Value)}");

            if (req.Content != null)
            {
                foreach (var header in req.Content.Headers)
                {
                    Debug.WriteLine($"{msg} {header.Key}: {string.Join(", ", header.Value)}");
                    apiDetails.Append($"{"Header Key "}{header.Key}");
                    apiDetails.Append($"{"Header Value "}{header.Value}");
                }

                if (req.Content is StringContent)
                {
                    var result = await req.Content.ReadAsStringAsync(cancellationToken);
                    var reqString = System.Text.Json.JsonSerializer.Serialize(result, System.Text.Json.JsonSerializerOptions.Default);

                    Debug.WriteLine($"{msg} Content:");
                    Debug.WriteLine($"{msg} {string.Join("", reqString)}");
                    apiDetails.Append($"{"Request "}{reqString}");
                }
                else
                {
                    string body = await request.Content.ReadAsStringAsync();

                    var reqString = System.Text.Json.JsonSerializer.Serialize(body, System.Text.Json.JsonSerializerOptions.Default);

                    Debug.WriteLine($"{msg} Content:");
                    Debug.WriteLine($"{msg} {string.Join("", reqString)}");
                    apiDetails.Append($"{"Request "}{reqString}");
                }
            }

            var start = DateTime.Now;
            var response = await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
            var end = DateTime.Now;

            Debug.WriteLine($"{msg} Duration: {end - start}");
            Debug.WriteLine($"{msg}==========End==========");
            apiDetails.Append($"{msg} Duration: {end - start}");

            msg = $"[{id} - Response]";
            Debug.WriteLine($"{msg}=========Start=========");
            apiDetails.Append($"{msg} [{id} - Response]");

            var resp = response;
            foreach (var header in resp.Headers)
            {
                Debug.WriteLine($"{msg} {header.Key}: {string.Join(", ", header.Value)}");
                apiDetails.Append($"{"Header Key "}{header.Key}");
                apiDetails.Append($"{"Header Value "}{header.Value}");
            }
            try
            {
                if (resp.Content != null)
                {
                    var result = await resp.Content.ReadAsStringAsync(cancellationToken);
                    start = DateTime.Now;
                    Debug.WriteLine($"{msg} Content:{result.ToString()}");
                    apiDetails.Append($"{"Content "}{result.ToString()}");
                    end = DateTime.Now;
                    Debug.WriteLine($"{msg} Duration: {end - start} Time span");
                    apiDetails.Append($"{"Duration "}{end - start} Time span");
                }
            }
            catch (Exception exception)
            {
                Debug.WriteLine("Failed - HandleException [{exceptionName}] \n{exceptionToString}", exception.GetType().Name, exception.ToString());
            }
            Debug.WriteLine($"{msg}==========End==========");
            Console.WriteLine(apiDetails);
            return response;
        }

        private static string Combine(string baseUrl, string relativeUrl) =>
         $"{baseUrl.TrimEnd('/')}/{relativeUrl.TrimStart('/')}";
    }
}
