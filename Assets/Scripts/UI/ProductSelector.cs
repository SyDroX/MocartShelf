using System;
using System.Collections;
using System.Collections.Generic;
using Entities;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
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
            _editButton.onClick.AddListener(OnEdit);
            _rightButton.onClick.AddListener(OnRight);
            _leftButton.onClick.AddListener(OnLeft);
            _receiver = MessageBroker.Default.Receive<List<Product>>().ObserveOnMainThread().Subscribe(OnProductsReceived);
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
            MessageBroker.Default.Publish(_products[_selectedIndex]);
        }

        private void OnLeft()
        {
            ToggleSelectedProduct(false);
            _selectedIndex = (_selectedIndex - 1 + _products.Count) % _products.Count;
            MessageBroker.Default.Publish(_products[_selectedIndex].ProductInfo);
            ToggleSelectedProduct(true);
        }

        private void OnRight()
        {
            ToggleSelectedProduct(false);
            _selectedIndex = (_selectedIndex + 1) % _products.Count;
            MessageBroker.Default.Publish(_products[_selectedIndex].ProductInfo);
            ToggleSelectedProduct(true);
        }

        private void ToggleButtons(bool state)
        {
            _rightButton.gameObject.SetActive(state);
            _leftButton.gameObject.SetActive(state);
        }

        private void ToggleSelectedProduct(bool state)
        {
            _products[_selectedIndex].Arrow.SetActive(state);
            _products[_selectedIndex].Rotator.enabled = state;
        }

        private void OnProductsReceived(List<Product> products)
        {
            _products = products;

            // TODO: comment why dis
            if (_products.Count == 1)
            {
                ToggleButtons(false);
                _selectedIndex = 0;
            }
            else
            {
                ToggleButtons(true);
                _selectedIndex = 1;
            }

            MessageBroker.Default.Publish(_products[_selectedIndex].ProductInfo);
            ToggleSelectedProduct(true);
        }
    }
}