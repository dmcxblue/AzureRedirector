using System;
using System.Net.Http;
using System.Security.Authentication;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Redirector.API
{
    public class ReverseProxy
    {
        // Hardcoded IP of the target server
        public static string TeamServer => "<TARGET IP>"; // Replace with your target IP

        private static readonly HttpClient _httpClient = new HttpClient(new HttpClientHandler
        {
            SslProtocols = SslProtocols.Tls12 | SslProtocols.Tls13, ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true // Accept all TLS Traffic
        });

        public static async Task Invoke(HttpContext context)
        {
            // Build the target URL with HTTPS
            var targetUri = new Uri($"https://{TeamServer}{context.Request.Path}{context.Request.QueryString}");

            var request = new HttpRequestMessage
            {
                Method = new HttpMethod(context.Request.Method),
                RequestUri = targetUri,
                Content = context.Request.ContentLength > 0 ? new StreamContent(context.Request.Body) : null
            };

            // Forward headers
            foreach (var header in context.Request.Headers)
            {
                if (!request.Headers.TryAddWithoutValidation(header.Key, header.Value.ToArray()))
                {
                    request.Content?.Headers.TryAddWithoutValidation(header.Key, header.Value.ToArray());
                }
            }

            var remoteResponse = await _httpClient.SendAsync(request);

            context.Response.StatusCode = (int)remoteResponse.StatusCode;

            foreach (var header in remoteResponse.Headers)
            {
                context.Response.Headers[header.Key] = header.Value.ToArray();
            }

            if (remoteResponse.Content != null)
            {
                await remoteResponse.Content.CopyToAsync(context.Response.Body);
            }
        }
    }
}
