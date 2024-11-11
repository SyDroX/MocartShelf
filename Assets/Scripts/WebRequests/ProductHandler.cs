/*using System;
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

        public static ProductInfo[] Get()
        {
            string response      = Get("Products");
            var    productsArray = JsonConvert.DeserializeObject<ProductsArray>(response);

            if (productsArray.Products == null)
            {
                throw new JsonException("Error parsing products");
            }
            
            return productsArray.Products;
        }
    }
}*/