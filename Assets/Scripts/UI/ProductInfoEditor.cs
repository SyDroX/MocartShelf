using System;
using System.Globalization;
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
        private Product     _productToEdit;

        [SerializeField] private TMP_InputField _nameInputField;
        [SerializeField] private TMP_InputField _priceInputField;
        [SerializeField] private TMP_InputField _descriptionInputField;
        [SerializeField] private Button         _saveButton;

        private void OnEnable()
        {
            _saveButton.onClick.AddListener(OnSaveClick);
            _receiver = MessageBroker.Default.Receive<Product>().ObserveOnMainThread().Subscribe(OnEditProduct);
        }

        private void OnDisable()
        {
            _receiver.Dispose();
            _saveButton.onClick.RemoveListener(OnSaveClick);
        }

        private void OnSaveClick()
        {
            _productToEdit.ProductInfo.Description = _descriptionInputField.text;
            _productToEdit.ProductInfo.Price       = decimal.Parse(_priceInputField.text);
            _productToEdit.ProductInfo.Name        = _nameInputField.text;

            MessageBroker.Default.Publish(_productToEdit.ProductInfo);
        }

        private void OnEditProduct(Product product)
        {
            _productToEdit              = product;
            _nameInputField.text        = _productToEdit.ProductInfo.Name;
            _priceInputField.text       = _productToEdit.ProductInfo.Price.ToString(CultureInfo.InvariantCulture);
            _descriptionInputField.text = _productToEdit.ProductInfo.Description;
        }
    }
}