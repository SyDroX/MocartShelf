using System;
using Entities;
using Newtonsoft.Json;

namespace WebRequests
{
    public class ProductJsonHelper
    {
        [Serializable]
        private class ProductsArray
        {
            public ProductInfo[] Products;
        }

        public static ProductInfo[] Deserialize(string productInfoJson)
        {
            var productsArray = JsonConvert.DeserializeObject<ProductsArray>(productInfoJson);

            if (productsArray.Products == null)
            {
                throw new JsonException("Error parsing products");
            }

            return productsArray.Products;
        }
    }
}