using System;
using System.Globalization;
using Entities;
using EventData;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class ProductInfoEditor : MonoBehaviour
    {
        private IDisposable _receiver;
        private ProductInfo _productInfoToEdit;

        [SerializeField] private TMP_InputField _nameInputField;
        [SerializeField] private TMP_InputField _priceInputField;
        [SerializeField] private TMP_InputField _descriptionInputField;
        [SerializeField] private Button         _saveButton;

        private void OnEnable()
        {
            _saveButton.onClick.AddListener(OnSaveClick);
            _receiver = MessageBroker.Default.Receive<EditProductInfoEventArgs>().ObserveOnMainThread().Subscribe(OnEditProduct);
        }

        private void OnDisable()
        {
            _receiver.Dispose();
            _saveButton.onClick.RemoveListener(OnSaveClick);
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
            _priceInputField.text       = _productInfoToEdit.Price.ToString(CultureInfo.InvariantCulture);
            _descriptionInputField.text = _productInfoToEdit.Description;
        }
    }
}