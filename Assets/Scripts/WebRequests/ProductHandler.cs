using System;
using System.Threading.Tasks;
using Entities;
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

            if (productsArray.Products == null)
            {
                throw new JsonException("Error parsing products");
            }
            
            return productsArray.Products;
        }
    }
}