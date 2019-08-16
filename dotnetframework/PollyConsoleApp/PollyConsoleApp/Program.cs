using Polly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PollyConsoleApp
{
    class Program
    {
        private static int totalRequests;
        private static int eventualSuccesses;
        private static int retries;
        private static int eventualFailures;

        static void Main(string[] args)
        {

            PollyWaitAndRetryAsync pollyWaitAndRetryAsync = new PollyWaitAndRetryAsync();
            string url = "http://http://asdasfadfdf.com/";
            pollyWaitAndRetryAsync.ExecuteAsync(new CancellationToken(), url).GetAwaiter().GetResult();
        }
    }
}
