using System;
using Entities;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class ProductInfoEditor : MonoBehaviour
    {
        private IDisposable _receiver;
        private Product _productToEdit;
        
        [SerializeField] private TMP_InputField _nameInputField;
        [SerializeField] private TMP_InputField _priceInputField;
        [SerializeField] private TMP_InputField _descriptionInputField;
        [SerializeField] private Button         _saveButton;
        [SerializeField] private Button         _cancelbButton;

        private void OnEnable()
        {
            _saveButton.onClick.AddListener(OnSaveClick);
            _cancelbButton.onClick.AddListener(OnCancelClick);
            _receiver = MessageBroker.Default.Receive<Product>().ObserveOnMainThread().Subscribe(OnEditProduct);
        }

        private void OnDisable()
        {
            _receiver.Dispose();
            _saveButton.onClick.RemoveListener(OnSaveClick);
            _cancelbButton.onClick.RemoveListener(OnCancelClick);
        }

        private void OnSaveClick()
        {
            // Send save event
            _productToEdit.ProductInfo.Description = _descriptionInputField.text;
            _productToEdit.ProductInfo.Price = decimal.Parse(_priceInputField.text);
            _productToEdit.ProductInfo.Name = _nameInputField.text;
        }

        private void OnCancelClick()
        {
            // Send cancel event
        }

        private void OnEditProduct(Product product)
        {
            _productToEdit = product;
        }
    }
}