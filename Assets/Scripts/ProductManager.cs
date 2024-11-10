using System.Collections.Generic;
using UnityEngine;
using WebRequests;

public class ProductManager : MonoBehaviour
{
    private Dictionary<int, Product> _products;
    private GameObject[] _productGameObjects;
    
    private void Start()
    {
        LoadProducts();
    }

    private void TryAddProduct(ProductInfo product)
    {
        if (int.TryParse(product.Name.Split(' ')[1], out int productIndex))
        {
            // Convert from displayed to zero based array
            productIndex -= 1;
            
        }
        else
        {
            Debug.LogError("Error phrasing product number");
        }
    }
    
    public async void LoadProducts()
    {
        // TODO: error handling
        ProductInfo[] products = await ProductHandler.Get();
        
        for (var i = 0; i < products.Length; i++)
        {
            
        }
    }
}