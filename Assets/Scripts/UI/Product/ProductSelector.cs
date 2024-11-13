using System;
using System.Collections.Generic;
using Entities;
using EventData;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Product
{
    public class ProductSelector : MonoBehaviour
    {
        private IDisposable         _receiver;
        private List<MocratProduct> _products;
        private int                 _selectedIndex;

        [SerializeField] private Button _leftButton;
        [SerializeField] private Button _rightButton;
        [SerializeField] private Button _editButton;

        private void OnEnable()
        {
            ToggleButtons(false);
            _editButton.onClick.AddListener(OnEdit);
            _rightButton.onClick.AddListener(OnRight);
            _leftButton.onClick.AddListener(OnLeft);
            _receiver = MessageBroker.Default.Receive<DisplayedProductsEventArgs>().ObserveOnMainThread().Subscribe(OnProductsReceived);
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
            MessageBroker.Default.Publish(new SelectedProductPositionEventArgs
            {
                Position = _products[_selectedIndex].GameObject.transform.position
            });
        }

        private void OnRight()
        {
            ToggleSelectedProduct(false);
            _selectedIndex = (_selectedIndex + 1) % _products.Count;
            MessageBroker.Default.Publish(new DisplayProductInfoEventArgs { ProductInfo = _products[_selectedIndex].ProductInfo });
            ToggleSelectedProduct(true);
            MessageBroker.Default.Publish(new SelectedProductPositionEventArgs
            {
                Position = _products[_selectedIndex].GameObject.transform.position
            });
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

        private void OnProductsReceived(DisplayedProductsEventArgs args)
        {
            ToggleSelectedProduct(false);
            _products      = new List<MocratProduct>(args.DisplayedProducts);
            _selectedIndex = 0;
            ToggleButtons(_products.Count != 1);
            ToggleSelectedProduct(true);
            MessageBroker.Default.Publish(new DisplayProductInfoEventArgs { ProductInfo = _products[_selectedIndex].ProductInfo });
            MessageBroker.Default.Publish(new SelectedProductPositionEventArgs
            {
                Position = _products[_selectedIndex].GameObject.transform.position
            });
        }
    }
}