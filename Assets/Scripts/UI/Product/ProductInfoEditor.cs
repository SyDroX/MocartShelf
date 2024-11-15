﻿using System;
using System.Globalization;
using Entities;
using EventData;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Product
{
    public class ProductInfoEditor : MonoBehaviour
    {
        private IDisposable _receiver;
        private ProductInfo _productInfoToEdit;
        private decimal     _productPrice;

        [SerializeField] private TMP_InputField _nameInputField;
        [SerializeField] private TMP_InputField _priceInputField;
        [SerializeField] private TMP_InputField _descriptionInputField;
        [SerializeField] private Button         _saveButton;

        private void OnEnable()
        {
            _priceInputField.onEndEdit.AddListener(OnPriceChanged);
            _saveButton.onClick.AddListener(OnSaveClick);
            _receiver = MessageBroker.Default.Receive<EditProductInfoEventArgs>().ObserveOnMainThread().Subscribe(OnEditProduct);
        }

        private void OnDisable()
        {
            _priceInputField.onEndEdit.RemoveListener(OnPriceChanged);
            _saveButton.onClick.RemoveListener(OnSaveClick);
            _receiver.Dispose();
        }

        private void OnPriceChanged(string newValue)
        {
            bool success = decimal.TryParse(newValue, out decimal newPrice);

            if (!success || newPrice <= 0)
            {
                _priceInputField.text = _productPrice.ToString(CultureInfo.InvariantCulture);
                MessageBroker.Default.Publish(new MessageEventArgs
                {
                    Message     = "Price must be a positive number.",
                    MessageType = MessageType.Error
                });
            }
        }

        private void OnSaveClick()
        {
            _productInfoToEdit.Description = _descriptionInputField.text;
            _productInfoToEdit.Price       = decimal.Parse(_priceInputField.text);
            _productInfoToEdit.Name        = _nameInputField.text;

            MessageBroker.Default.Publish(new DisplayProductInfoEventArgs { ProductInfo = _productInfoToEdit });
            MessageBroker.Default.Publish(new MessageEventArgs
            {
                Message     = "Saved!",
                MessageType = MessageType.Success
            });
        }

        private void OnEditProduct(EditProductInfoEventArgs args)
        {
            _productInfoToEdit          = args.Product;
            _nameInputField.text        = _productInfoToEdit.Name;
            _productPrice               = _productInfoToEdit.Price;
            _priceInputField.text       = _productInfoToEdit.Price.ToString(CultureInfo.InvariantCulture);
            _descriptionInputField.text = _productInfoToEdit.Description;
        }
    }
}