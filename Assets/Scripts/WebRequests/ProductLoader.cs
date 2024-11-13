using System;
using System.Collections;
using System.Collections.Generic;
using Entities;
using EventData;
using Newtonsoft.Json;
using UniRx;
using UnityEngine;
using UnityEngine.Networking;

namespace WebRequests
{
    public class ProductLoader : MonoBehaviour
    {
        // Since there is only one entry point to access the API I'm hardcoding the uri, this should come form a configuration file
        // and have a base uri and uri pages for each api method.
        private const string ApiUri = "https://homework.mocart.io/api/products";

        private bool                _loadInProgress;
        private IDisposable         _receiver;
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

        private static void ToggleLoadingPanel(bool enabled)
        {
            MessageBroker.Default.Publish(new LoadingPanelEventArgs { Enabled = enabled });
        }

        private static void HandleError(string message, bool debugLog = true, string debugLogMessage = "")
        {
            if (debugLog)
            {
                Debug.LogError(debugLogMessage == string.Empty ? message : debugLogMessage);
            }

            MessageBroker.Default.Publish(new MessageEventArgs
            {
                Message     = message,
                MessageType = MessageType.Error
            });
        }

        private static void HandleException(string message, Exception exception)
        {
            Debug.LogError(exception.Message + "\n" + exception.StackTrace);
            MessageBroker.Default.Publish(new MessageEventArgs
            {
                Message     = message,
                MessageType = MessageType.Error
            });
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

                HandleError($"Error, Product '{productInfo.Name}' is not a number");
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
                HandleException("Error reading product data!", jsonException);
            }
            catch (Exception exception)
            {
                HandleException("Unknown Error!", exception);
            }
            finally
            {
                ToggleLoadingPanel(false);
                _loadInProgress = false;
            }
        }

        private IEnumerator RequestProductsFromServer(string uri)
        {
            ToggleLoadingPanel(true);

            using UnityWebRequest webRequest = UnityWebRequest.Get(uri);

            yield return webRequest.SendWebRequest();

            if (webRequest.Succeeded())
            {
                HandleServerResponse(webRequest.downloadHandler.text);
            }
            else
            {
                _loadInProgress = false;
                HandleError("Error getting products from server!", true, $"{uri} - Error {webRequest.result}: {webRequest.error}");
                ToggleLoadingPanel(false);
            }
        }

        public void LoadProducts()
        {
            if (!_loadInProgress)
            {
                _loadInProgress = true;
                StartCoroutine(RequestProductsFromServer(ApiUri));
            }
        }
    }
}