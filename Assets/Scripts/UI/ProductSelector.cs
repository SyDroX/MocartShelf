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
        [SerializeField] private float  _highlightTimeSeconds  = 1f;
        [SerializeField] private float  _highlightMaxIntensity = 1f;

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

        private IEnumerator LerpLightIntensity(float from, float to, Light targetLight)
        {
            var time = 0f;

            while (time < _highlightTimeSeconds)
            {
                time                  += Time.deltaTime;
                targetLight.intensity =  Mathf.Lerp(from, to, time / _highlightTimeSeconds);

                yield return null;
            }
        }


        private void OnEdit()
        {
            MessageBroker.Default.Publish(_products[_selectedIndex]);
        }

        private void OnLeft()
        {
            StartCoroutine(LerpLightIntensity(_highlightMaxIntensity, 0, _products[_selectedIndex].Highlighter));
            _selectedIndex = (_selectedIndex - 1 + _products.Count) % _products.Count;
            MessageBroker.Default.Publish(_products[_selectedIndex].ProductInfo);
            StartCoroutine(LerpLightIntensity(0, _highlightMaxIntensity, _products[_selectedIndex].Highlighter));
        }

        private void OnRight()
        {
            StartCoroutine(LerpLightIntensity(_highlightMaxIntensity, 0, _products[_selectedIndex].Highlighter));
            _selectedIndex = (_selectedIndex + 1) % _products.Count;
            MessageBroker.Default.Publish(_products[_selectedIndex].ProductInfo);
            StartCoroutine(LerpLightIntensity(0, _highlightMaxIntensity, _products[_selectedIndex].Highlighter));
        }

        private void ToggleButtons(bool state)
        {
            _rightButton.gameObject.SetActive(state);
            _leftButton.gameObject.SetActive(state);
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
            StartCoroutine(LerpLightIntensity(0, _highlightMaxIntensity, _products[_selectedIndex].Highlighter));
        }
    }
}