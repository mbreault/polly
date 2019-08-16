using Polly;
using Polly.Retry;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

// Based on code from https://github.com/App-vNext/Polly-Samples

namespace PollyConsoleApp
{
    public class PollyWaitAndRetryAsync
    {
        private int _retries;
        private int _totalRequests;
        private int _eventualSuccesses;
        private int _eventualFailures;
        private AsyncRetryPolicy _policy;

        public PollyWaitAndRetryAsync()
        {
            _retries = 0;
            _totalRequests = 0;
            _eventualSuccesses = 0;
            _eventualFailures = 0;

            Random jitterer = new Random();

            _policy = Policy.Handle<Exception>().WaitAndRetryAsync(
                      retryCount: 3,
                      sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                      onRetry: (exception, calculatedWaitDuration) => // Capture some info for logging!
                      {
                          Console.WriteLine("Policy logging: " + exception.Message);
                          _retries++;
                      });
        }

        public async Task ExecuteAsync(CancellationToken cancellationToken, string requestURI)
        {
            using (var client = new HttpClient())
            {

                _totalRequests++;

                try
                {
                    // Retry the following call according to the policy 
                    await _policy.ExecuteAsync(async token =>
                    {
                        // This code is executed within the Policy 

                        // Make a request and get a response
                        string msg = await client.GetStringAsync(requestURI);

                        // Display the response message on the console
                        Console.WriteLine("Response : " + msg);
                        _eventualSuccesses++;
                    }, cancellationToken);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Request " + _totalRequests + " eventually failed with: " + e.Message);
                    _eventualFailures++;
                }
            }
        }
    }
}

