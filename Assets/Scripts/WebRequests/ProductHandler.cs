using System.Threading.Tasks;
using Newtonsoft.Json;

namespace WebRequests
{
    public class ProductHandler : HttpRequestHandler
    {
        public static async Task<Product[]> Get()
        {
            string    response = await GetAsync($"Products");
            Product[] products = JsonConvert.DeserializeObject<Product[]>(response);

            return products;
        }
    }
}