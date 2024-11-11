using System;
using System.Collections.Generic;
using Entities;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class DisplayProductInfoEventArgs
    {
        public ProductInfo ProductInfo;
    }

    public class EditProductInfoEventArgs
    {
        public ProductInfo Product;
    }
    
    public class ProductSelector : MonoBehaviour
    {
        private IDisposable   _receiver;
        private List<Product> _products;
        private int           _selectedIndex;

        [SerializeField] private Button _leftButton;
        [SerializeField] private Button _rightButton;
        [SerializeField] private Button _editButton;

        private void OnEnable()
        {
            ToggleButtons(false);
            _editButton.onClick.AddListener(OnEdit);
            _rightButton.onClick.AddListener(OnRight);
            _leftButton.onClick.AddListener(OnLeft);
            _receiver = MessageBroker.Default.Receive<LoadedProductsEventArgs>().ObserveOnMainThread().Subscribe(OnProductsReceived);
        }

        private void OnDisable()
        {
            _editButton.onClick.RemoveListener(OnEdit);
            _rightButton.onClick.RemoveListener(OnRight);
            _leftButton.onClick.RemoveListener(OnLeft);
            _receiver.Dispose();
        }

        private void OnEdit()
        {
            MessageBroker.Default.Publish(new EditProductInfoEventArgs { Product = _products[_selectedIndex].ProductInfo });
        }

        private void OnLeft()
        {
            ToggleSelectedProduct(false);
            _selectedIndex = (_selectedIndex - 1 + _products.Count) % _products.Count;
            MessageBroker.Default.Publish(new DisplayProductInfoEventArgs { ProductInfo = _products[_selectedIndex].ProductInfo });
            ToggleSelectedProduct(true);
        }

        private void OnRight()
        {
            ToggleSelectedProduct(false);
            _selectedIndex = (_selectedIndex + 1) % _products.Count;
            MessageBroker.Default.Publish(new DisplayProductInfoEventArgs { ProductInfo = _products[_selectedIndex].ProductInfo });
            ToggleSelectedProduct(true);
        }

        private void ToggleButtons(bool state)
        {
            _rightButton.gameObject.SetActive(state);
            _leftButton.gameObject.SetActive(state);
        }

        private void ToggleSelectedProduct(bool state)
        {
            if (_products != null)
            {
                _products[_selectedIndex].Arrow.SetActive(state);
                _products[_selectedIndex].Rotator.enabled = state;
            }
        }

        private void OnProductsReceived(LoadedProductsEventArgs args)
        {
            ToggleSelectedProduct(false);
            _products      = new List<Product>(args.LoadedProducts);
            _selectedIndex = 0;
            ToggleButtons(_products.Count != 1);
            ToggleSelectedProduct(true);
            MessageBroker.Default.Publish(new DisplayProductInfoEventArgs { ProductInfo = _products[_selectedIndex].ProductInfo });
        }
    }
}