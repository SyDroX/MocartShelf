using System;
using System.Collections;
using System.Collections.Generic;
using Entities;
using EventData;
using Newtonsoft.Json;
using UniRx;
using UnityEngine;
using UnityEngine.Networking;
/*using WebRequests;*/

public class ProductLoader : MonoBehaviour
{
    private const string ApiUri = "https://homework.mocart.io/api/products";
    
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
            Debug.LogError("Error phrasing product number");
        }
    }

    [Serializable]
    private class ProductsArray
    {
        public ProductInfo[] Products;
    }

    private IEnumerator Load(string uri)
    {
        MessageBroker.Default.Publish(new LoadingPanelEventArgs { Enabled = true });
    
        // I know this implementation isn't great, I had to scrap my httpclient handler due to lack of support in webGL last moment.
        // Had I known this from the start I'd use UniRX's implementation for this
        // https://www.gokhand.com/blog/using-unirx-to-perform-web-requests-in-unity
        using UnityWebRequest webRequest = UnityWebRequest.Get(uri);

        yield return webRequest.SendWebRequest();

        string[] pages = uri.Split('/');
        int      page  = pages.Length - 1;
        
        switch (webRequest.result)
        {
            case UnityWebRequest.Result.ConnectionError:
            case UnityWebRequest.Result.DataProcessingError:
                Debug.LogError(pages[page] + ": Error: " + webRequest.error);
                MessageBroker.Default.Publish(new MessageEventArgs
                {
                    Message     = "Error getting products!",
                    MessageType = MessageType.Error
                });
                break;
            case UnityWebRequest.Result.ProtocolError:
                Debug.LogError(pages[page] + ": HTTP Error: " + webRequest.error);
                MessageBroker.Default.Publish(new MessageEventArgs
                {
                    Message     = "Error getting products!",
                    MessageType = MessageType.Error
                });
                break;
        }
        
        try
        {
            var productsArray = JsonConvert.DeserializeObject<ProductsArray>(webRequest.downloadHandler.text);

            ProductInfo[] productInfos = productsArray.Products;
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
                Message     = "Error reading products!",
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

    public void LoadProducts()
    {
        StartCoroutine(Load(ApiUri));
        /*MessageBroker.Default.Publish(new LoadingPanelEventArgs { Enabled = true });

        try
        {
            ProductInfo[] productInfos = ProductHandler.Get();
            _loadedProducts.Clear();

            foreach (ProductInfo productInfo in productInfos)
            {
                TryAddProduct(productInfo);
            }

            MessageBroker.Default.Publish(new LoadedProductsEventArgs { LoadedProducts = _loadedProducts });
        }
        catch (HttpRequestException httpRequestException)
        {
            MessageBroker.Default.Publish(new MessageEventArgs
            {
                Message     = "Error getting products!",
                MessageType = MessageType.Error
            });
            Debug.LogError(httpRequestException.Message + "\n" + httpRequestException.StackTrace);
        }
        catch (JsonException jsonException)
        {
            MessageBroker.Default.Publish(new MessageEventArgs
            {
                Message     = "Error reading products!",
                MessageType = MessageType.Error
            });
            Debug.LogError(jsonException.Message + "\n" + jsonException.StackTrace);
        }
        catch (Exception exception)
        {
            MessageBroker.Default.Publish(new MessageEventArgs
            {
                //Message     = "Unknown Error!",
                Message = exception.Message + "\n" + exception.StackTrace,
                MessageType = MessageType.Error
            });
            Debug.LogError(exception.Message + "\n" + exception.StackTrace);
        }
        finally
        {
            MessageBroker.Default.Publish(new LoadingPanelEventArgs { Enabled = false });
        }*/
    }
}