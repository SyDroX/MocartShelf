using System;
using System.Collections;
using System.Collections.Generic;
using Entities;
using EventData;
using Newtonsoft.Json;
using UniRx;
using UnityEngine;
using UnityEngine.Networking;
using WebRequests;

public class ProductLoader : MonoBehaviour
{
    private const string ApiUri          = "https://homework.mocart.io/api/products";
    private const string ProductRequestError = "Error getting products from server!";
    
    private IDisposable   _receiver;
    private MocratProduct[]     _products;
    private List<MocratProduct> _loadedProducts = new();

    private void OnEnable()
    {
        _receiver = MessageBroker.Default.Receive<ProductsPooledEventArgs>().ObserveOnMainThread().Subscribe(OnProductPoolCreated);
    }

    private void OnDisable()
    {
        _receiver.Dispose();
    }

    private void OnProductPoolCreated(ProductsPooledEventArgs args)
    {
        _products = args.ProductsPool;
        LoadProducts();
    }

    private void TryAddProduct(ProductInfo productInfo)
    {
        if (int.TryParse(productInfo.Name.Split(' ')[1], out int productIndex))
        {
            // Convert from displayed to zero based array
            productIndex -= 1;
            // Using the product name as ID, this allows the changes made to product info to persist between reloads
            _products[productIndex].ProductInfo ??= productInfo;
            _loadedProducts.Add(_products[productIndex]);
        }
        else
        {
            var errorMessage = $"Error, Product '{productInfo.Name}' is not a number";
            Debug.LogError(errorMessage);
            MessageBroker.Default.Publish(new MessageEventArgs
            {
                Message     = errorMessage,
                MessageType = MessageType.Error
            });
        }
    }

    private void HandleServerResponse(string productData)
    {
        try
        {
            ProductInfo[] productInfos = ProductJsonHelper.Deserialize(productData);
            _loadedProducts.Clear();

            foreach (ProductInfo productInfo in productInfos)
            {
                TryAddProduct(productInfo);
            }

            MessageBroker.Default.Publish(new LoadedProductsEventArgs { LoadedProducts = _loadedProducts });
        }
        catch (JsonException jsonException)
        {
            MessageBroker.Default.Publish(new MessageEventArgs
            {
                Message     = "Error reading product data!",
                MessageType = MessageType.Error
            });
            Debug.LogError(jsonException.Message + "\n" + jsonException.StackTrace);
        }
        catch (Exception exception)
        {
            MessageBroker.Default.Publish(new MessageEventArgs
            {
                Message     = "Unknown Error!",
                MessageType = MessageType.Error
            });
            Debug.LogError(exception.Message + "\n" + exception.StackTrace);
        }
        finally
        {
            MessageBroker.Default.Publish(new LoadingPanelEventArgs { Enabled = false });
        }
    }

    private IEnumerator RequestProductsFromServer(string uri)
    {
        MessageBroker.Default.Publish(new LoadingPanelEventArgs { Enabled = true });
        
        using UnityWebRequest webRequest = UnityWebRequest.Get(uri);
        
        yield return webRequest.SendWebRequest();

        if (webRequest.Succeeded())
        {
            HandleServerResponse(webRequest.downloadHandler.text);
        }
        else
        {
            Debug.LogError(uri + " " + webRequest.result + ": Error: " + webRequest.error);
            MessageBroker.Default.Publish(new MessageEventArgs
            {
                Message     = ProductRequestError,
                MessageType = MessageType.Error
            });
        }
    }

    public void LoadProducts()
    {
        StartCoroutine(RequestProductsFromServer(ApiUri));
    }
}