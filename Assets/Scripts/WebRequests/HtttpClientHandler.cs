using System.Net.Http;

namespace WebRequests
{
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
    }
}