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

        private static async Task<string> HandleResponseAsync(HttpResponseMessage httpResponseMessage)
        {
            if (httpResponseMessage.StatusCode != HttpStatusCode.OK)
            {
                throw new HttpRequestException(httpResponseMessage.StatusCode + " - " + httpResponseMessage.Content);
            }

            await using Stream responseStream = await httpResponseMessage.Content.ReadAsStreamAsync();
            using var          streamReader   = new StreamReader(responseStream);

            return await streamReader.ReadToEndAsync();
        }

        protected static async Task<string> GetAsync(string requestUri)
        {
            using HttpResponseMessage httpResponseMessage = await SharedClient.GetAsync(string.Concat(MocartUri, requestUri));

            return await HandleResponseAsync(httpResponseMessage);
        }
    }
}