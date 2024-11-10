using UniRx;
using UnityEngine;

public class ProductPool : MonoBehaviour
{
    [SerializeField] private GameObject[] _productPrefabs;

    private void Start()
    {
        // Show Loading 
        InstantiateProductsPool();
        // Hide Loading 
    }

    private void InstantiateProductsPool()
    {
        var       productsPool    = new Product[_productPrefabs.Length];
        Transform productPoolRoot = new GameObject("ProductPool").transform;

        for (var index = 0; index < _productPrefabs.Length; index++)
        {
            productsPool[index] = new Product { GameObject = Instantiate(_productPrefabs[index], productPoolRoot) };
            productsPool[index].GameObject.SetActive(false);
        }

        MessageBroker.Default.Publish(productsPool);
    }
}
