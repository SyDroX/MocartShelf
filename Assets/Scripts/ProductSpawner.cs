using System.Collections.Generic;
using UniRx;
using UnityEngine;
using WebRequests;

public class ProductSpawner : MonoBehaviour
{
    [SerializeField] private Vector3      _initialPosition;
    [SerializeField] private float        _offsetX;
    [SerializeField] private GameObject[] _productPrefabs;

    private GameObject[] _productsPool;
    private List<int>    _spawnedProductIndexes;

    private void Start()
    {
        _spawnedProductIndexes = new List<int>();
        InstantiateProductsPool();
        SpawnProducts();
    }

    public async void SpawnProducts()
    {
        _spawnedProductIndexes.Clear();

        foreach (GameObject product in _productsPool)
        {
            product.SetActive(false);
        }

        // TODO: error handling
        Product[] products = await ProductHandler.Get();
        
        for (var i = 0; i < products.Length; i++)
        {
            if (int.TryParse(products[i].Name.Split(' ')[1], out int productIndex))
            {
                // Convert from displayed to zero based array
                productIndex -= 1;

                _productsPool[productIndex].transform.position = _initialPosition + new Vector3(_offsetX * i, 0, 0);
                _productsPool[productIndex].SetActive(true);
                _spawnedProductIndexes.Add(productIndex);
            }
            else
            {
                Debug.LogError("Error phrasing product number");
            }
        }
        
        MessageBroker.Default.Publish(new List<Transform>());
    }

    private void InstantiateProductsPool()
    {
        _productsPool = new GameObject[_productPrefabs.Length];
        Transform productPoolRoot = new GameObject("ProductPool").transform;

        for (var i = 0; i < _productPrefabs.Length; i++)
        {
            _productsPool[i] = Instantiate(_productPrefabs[i], productPoolRoot);
        }
    }
}
