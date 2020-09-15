using System;
using System.Net.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Extensions.Http;

namespace Contact.API.Policies
{
    public class DefaultPolicy
    {
        private readonly ILogger<DefaultPolicy> _logger;
        //private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly int _retryCount;
        //熔断之前允许的异常次数
        private readonly int _exceptionCountAllowedBeforeBreaking;

        public DefaultPolicy(ILogger<DefaultPolicy> logger, int retryCount, int exceptionCountAllowedBeforeBreaking)
        {
            _logger = logger;
            //_httpContextAccessor = httpContextAccessor;
            _retryCount = retryCount;
            _exceptionCountAllowedBeforeBreaking = exceptionCountAllowedBeforeBreaking;
        }

        public static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy(int retryCount)
        {           
            Random jitterer = new Random();
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound)
                .WaitAndRetryAsync(
                        // number of retries
                        retryCount,
                        // exponential backofff
                        retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))
                            + TimeSpan.FromMilliseconds(jitterer.Next(0, 100)),
                        // on retry
                        (exception, timeSpan, retryCount, context) =>
                        {
                            var msg = $"Retry {retryCount} implemented with Polly's RetryPolicy " +
                                      $"of {context.PolicyKey} " +
                                      $"at {context.OperationKey}, " +
                                      $"due to: {exception}.";
                            // _logger.LogWarning(msg);
                            // _logger.LogDebug(msg);
                        });
        }
        public static IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy(int exceptionCountAllowedBeforeBreaking)
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .CircuitBreakerAsync(
                    // number of exceptions before breaking circuit
                    exceptionCountAllowedBeforeBreaking,
                    // time circuit opened before retry
                    TimeSpan.FromMinutes(1),
                    (exception, duration) =>
                    {
                        // on circuit opened
                        //_logger.LogTrace("Circuit breaker opened");
                    },
                    () =>
                    {
                        // on circuit closed
                       // _logger.LogTrace("Circuit breaker reset");
                    });
        }
    }
}
