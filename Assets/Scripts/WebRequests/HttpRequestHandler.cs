using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace WebRequests
{
    public abstract class HttpRequestHandler
    {
        private static readonly HttpClient SharedClient = HttpClientHandler.Instance.SharedClient;
        // Since this is a homework assigment with single backend api entry point I'm hardcoding it.
        // In a more complete solution this can be read from a configuration file
        private const string MocartUri = "https://homework.mocart.io/api/";

        private static string HandleResponse(HttpResponseMessage httpResponseMessage)
        {
            // TODO: figure out what to return on error
            if (httpResponseMessage.StatusCode != HttpStatusCode.OK) return string.Empty;

            using Stream responseStream = httpResponseMessage.Content.ReadAsStreamAsync().Result;
            using var    streamReader   = new StreamReader(responseStream);

            return streamReader.ReadToEnd();
        }

        private static async Task<string> HandleResponseAsync(HttpResponseMessage httpResponseMessage)
        {
            // TODO: figure out what to return on error
            if (httpResponseMessage.StatusCode != HttpStatusCode.OK) return string.Empty;

            await using Stream responseStream = await httpResponseMessage.Content.ReadAsStreamAsync();
            using var          streamReader   = new StreamReader(responseStream);

            return await streamReader.ReadToEndAsync();
        }

        protected static string Get(string requestUri)
        {
            using HttpResponseMessage httpResponseMessage = SharedClient.GetAsync(string.Concat(MocartUri, requestUri)).Result;

            return HandleResponse(httpResponseMessage);
        }

        protected static async Task<string> GetAsync(string requestUri)
        {
            using HttpResponseMessage httpResponseMessage = await SharedClient.GetAsync(string.Concat(MocartUri, requestUri));

            return await HandleResponseAsync(httpResponseMessage);
        }
    }
}