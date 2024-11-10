using System;
using System.Collections.Generic;
using Entities;
using UniRx;
using UnityEngine;

public class ProductDisplayer : MonoBehaviour
{
    private IDisposable   _receiver;
    private List<Product> _shownProducts = new();

    [SerializeField] private Vector3 _initialPosition;
    [SerializeField] private float   _offsetX;

    private void OnEnable()
    {
        _receiver = MessageBroker.Default.Receive<List<Product>>().ObserveOnMainThread().Subscribe(OnProductsReceived);
    }

    private void OnDisable()
    {
        _receiver.Dispose();
    }

    private void OnProductsReceived(List<Product> products)
    {
        _shownProducts.ForEach(sp => sp.GameObject.SetActive(false));
        _shownProducts = new List<Product>(products);

        if (_shownProducts.Count == 1)
        {
            Product product = _shownProducts[0];
            product.GameObject.transform.position = _initialPosition + new Vector3(_offsetX, 0, 0);
            product.GameObject.SetActive(true);
        }
        else
        {
            for (var index = 0; index < _shownProducts.Count; index++)
            {
                Product product = _shownProducts[index];
                product.GameObject.transform.position = _initialPosition + new Vector3(_offsetX * index, 0, 0);
                product.GameObject.SetActive(true);
            }
        }
    }
}