using System;
using System.Collections.Generic;
using EventData;
using UniRx;
using UnityEngine;

namespace UI.Product
{
    public class ProductDisplayer : MonoBehaviour
    {
        private IDisposable            _receiver;
        private List<Entities.Product> _shownProducts = new();

        [SerializeField] private Vector3 _initialPosition;
        [SerializeField] private float   _offsetX;

        private void OnEnable()
        {
            _receiver = MessageBroker.Default.Receive<LoadedProductsEventArgs>().ObserveOnMainThread().Subscribe(OnProductsReceived);
        }

        private void OnDisable()
        {
            _receiver.Dispose();
        }

        private void OnProductsReceived(LoadedProductsEventArgs args)
        {
            _shownProducts.ForEach(sp => sp.GameObject.SetActive(false));
            _shownProducts = new List<Entities.Product>(args.LoadedProducts);

            for (var index = 0; index < _shownProducts.Count; index++)
            {
                Entities.Product product   = _shownProducts[index];
                float   positionX = _offsetX * (index - (_shownProducts.Count - 1) / 2.0f);
                product.GameObject.transform.position = _initialPosition + new Vector3(positionX, 0, 0);
                product.GameObject.SetActive(true);
            }
        }
    }
}