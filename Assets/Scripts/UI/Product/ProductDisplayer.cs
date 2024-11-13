using System;
using System.Collections.Generic;
using Entities;
using EventData;
using UniRx;
using UnityEngine;

namespace UI.Product
{
    public class ProductDisplayer : MonoBehaviour
    {
        private IDisposable                  _receiver;
        private List<MocratProduct> _shownProducts = new();

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
            _shownProducts = new List<MocratProduct>(args.LoadedProducts);

            for (var index = 0; index < _shownProducts.Count; index++)
            {
                MocratProduct mocratProduct = _shownProducts[index];
                float         positionX     = _offsetX * (index - (_shownProducts.Count - 1) / 2.0f);
                mocratProduct.GameObject.transform.position = _initialPosition + new Vector3(positionX, 0, 0);
                mocratProduct.GameObject.SetActive(true);
            }

            MessageBroker.Default.Publish(new DisplayedProductsEventArgs { DisplayedProducts = _shownProducts });
        }
    }
}