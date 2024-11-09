using System.Collections.Generic;
using UnityEngine;
using WebRequests;

public class ProductSpawner : MonoBehaviour
{
    [SerializeField] private Vector3 _initialPosition;
    [SerializeField] private float   _offsetX;
    [SerializeField] private float   _rotationSpeed = 1f;
    
    [SerializeField] private GameObject[] _productPrefabs;

    private GameObject[] _productsPool;
    private List<int>    _spawnedProductIndexes;

    private void Start()
    {
        _spawnedProductIndexes = new List<int>();
        InstantiateProducts();
        SpawnProducts();
    }
    private void Update()
    {
        _spawnedProductIndexes.ForEach(sp=> _productsPool[sp].transform.Rotate(new Vector3(0,1)* (Time.deltaTime * _rotationSpeed)));
    }

    public async void SpawnProducts()
    {
        _spawnedProductIndexes.Clear();
        
        foreach (GameObject product in _productsPool)
        {
            product.SetActive(false);
        }
        
        Product[] products = await ProductHandler.Get();

        for (var i = 0; i < products.Length; i++)
        {
            if (int.TryParse(products[i].Name.Split(' ')[1], out int productIndex))
            {
                // Convert from displayed to array
                productIndex -= 1;
                
                _productsPool[productIndex].transform.position =  _initialPosition + new Vector3(_offsetX * i, 0, 0);
                _productsPool[productIndex].SetActive(true);
                _spawnedProductIndexes.Add(productIndex);
            }
        }
    }

    private void InstantiateProducts()
    {
        _productsPool = new GameObject[_productPrefabs.Length];
        Transform productPoolRoot = new GameObject("ProductPool").transform;
        
        for (int i = 0; i < _productPrefabs.Length; i++)
        {
            _productsPool[i] = Instantiate(_productPrefabs[i],productPoolRoot);
        }
    }
}
