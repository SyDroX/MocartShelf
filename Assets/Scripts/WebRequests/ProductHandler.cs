using System;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace WebRequests
{
    public class ProductHandler : HttpRequestHandler
    {
        [Serializable]
        private class ProductsArray
        {
            public ProductInfo[] Products;
        }

        public static async Task<ProductInfo[]> Get()
        {
            string response      = await GetAsync("Products");
            var    productsArray = JsonConvert.DeserializeObject<ProductsArray>(response);

            return productsArray.Products;
        }
    }
}