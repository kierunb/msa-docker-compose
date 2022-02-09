using Polly;
using Polly.Extensions.Http;

namespace frontend
{
    public static class ResiliencyPolicies
    {
        public static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
            {
                return HttpPolicyExtensions
                    .HandleTransientHttpError()
                    .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound)
                    .WaitAndRetryAsync(6, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2,
                                                                                retryAttempt)));
            }
    }
}