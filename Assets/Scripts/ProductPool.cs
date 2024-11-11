using Entities;
using UniRx;
using UnityEditor;
using UnityEngine;

public class ProductPool : MonoBehaviour
{
    [SerializeField] private GameObject _arrowPrefab;
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
            GameObject productRoot     = new GameObject(_productPrefabs[index].name + " Root");
            GameObject productInstance = Instantiate(_productPrefabs[index], productRoot.transform);
            GameObject arrow           = Instantiate(_arrowPrefab,           productRoot.transform);
            productRoot.transform.SetParent(productPoolRoot);
            arrow.SetActive(false);
            productsPool[index] = new Product
            {
                GameObject = productRoot,
                Rotator    = productInstance.GetComponent<Rotator>(),
                Arrow      = arrow
            };
            
            productsPool[index].GameObject.SetActive(false);
        }

        MessageBroker.Default.Publish(productsPool);
    }
}
