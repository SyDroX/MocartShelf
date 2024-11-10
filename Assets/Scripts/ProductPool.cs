using System.Collections.Generic;
using UnityEngine;

public class ProductPool : MonoBehaviour
{
    [SerializeField] private Vector3      _initialPosition;
    [SerializeField] private float        _offsetX;
    [SerializeField] private GameObject[] _productPrefabs;

    private GameObject[] _productsPool;
    private List<int>    _spawnedProductIndexes;

    private void Awake()
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
