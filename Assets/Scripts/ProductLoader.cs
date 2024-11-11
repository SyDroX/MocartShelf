﻿using System;
using System.Collections.Generic;
using Entities;
using UI;
using UniRx;
using UnityEngine;
using WebRequests;

public class LoadedProductsEventArgs
{
    public List<Product> LoadedProducts;
}

public class ProductLoader : MonoBehaviour
{
    private IDisposable   _receiver;
    private Product[]     _products;
    private List<Product> _loadedProducts = new();

    private void OnEnable()
    {
        _receiver = MessageBroker.Default.Receive<ProductsPooledEventArgs>().ObserveOnMainThread().Subscribe(OnProductPoolCreated);
    }

    private void OnDisable()
    {
        _receiver.Dispose();
    }

    private void Start()
    {
        LoadProducts();
    }

    private void OnProductPoolCreated(ProductsPooledEventArgs args)
    {
        _products = args.ProductsPool;
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
        MessageBroker.Default.Publish(new LoadingPanelEventArgs { Enabled = true });
        // TODO: error handling
        ProductInfo[] productInfos = await ProductHandler.Get();

        _loadedProducts.Clear();

        foreach (ProductInfo productInfo in productInfos)
        {
            TryAddProduct(productInfo);
        }

        MessageBroker.Default.Publish(new LoadingPanelEventArgs { Enabled          = false });
        MessageBroker.Default.Publish(new LoadedProductsEventArgs { LoadedProducts = _loadedProducts });
    }
}