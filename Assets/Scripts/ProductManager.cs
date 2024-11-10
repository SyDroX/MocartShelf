using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using WebRequests;

public class ProductManager : MonoBehaviour
{
    private IDisposable _receiver;
    private Product[]   _products;
    private List<Product> _loadedProducts = new();
    
    private void OnEnable()
    {
        _receiver = MessageBroker.Default.Receive<Product[]>().ObserveOnMainThread().Subscribe(OnProductPoolCreated);
    }

    private void OnDisable()
    {
        _receiver.Dispose();
    }

    private void Start()
    {
        LoadProducts();
    }

    private void OnProductPoolCreated(Product[] products)
    {
        _products = products;
    }
    
    private void TryAddProduct(ProductInfo productInfo)
    {
        if (int.TryParse(productInfo.Name.Split(' ')[1], out int productIndex))
        {
            // Convert from displayed to zero based array
            productIndex                        -=  1;
            _products[productIndex].ProductInfo ??= productInfo;
            _loadedProducts.Add(_products[productIndex]);
        }
        else
        {
            Debug.LogError("Error phrasing product number");
        }
    }
    
    public async void LoadProducts()
    {
        // Show Loading
        // TODO: error handling
        ProductInfo[] productInfos = await ProductHandler.Get();
        
        _loadedProducts.Clear();
        
        foreach (ProductInfo productInfo in productInfos)
        {
            TryAddProduct(productInfo);
        }
        
        MessageBroker.Default.Publish(_loadedProducts);
        // Hide Loading
    }
}