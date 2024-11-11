/*using System.Net.Http;

namespace WebRequests
{
    // I opted to use .Net's http client for web requests. This is because it allows better control of API request life-cycle
    // In addition, it allows to run these requests in async interdependently on unity's life-cycle.
    public sealed class HttpClientHandler
    {
        public readonly HttpClient SharedClient;
    
        public static HttpClientHandler Instance { get; } = new();

        // Explicit static constructor to tell C# compiler
        // not to mark type as beforeFieldInit
        static HttpClientHandler()
        {
        }

        private HttpClientHandler()
        {
            SharedClient = new HttpClient();
        }

        // Unity's Editor life-cycle can't properly dispose of a shared client instance this persists in version 2019 to 2022,
        // Maybe it's fixed in the new version 6, haven't tested yet. However, this works fine in build.
#if !UNITY_EDITOR
        ~HttpClientHandler()
        {
             SharedClient.Dispose();
        }
#endif
    }
}*/