using System.Threading.Tasks;
using Newtonsoft.Json;

namespace WebRequests
{
    public class ProductHandler : HttpRequestHandler
    {
        public static async Task<Product[]> Get()
        {
            string    response = await GetAsync("Products");
            var productsArray = JsonConvert.DeserializeObject<ProductsArray>(response);

            return productsArray.Products;
        }
    }
}