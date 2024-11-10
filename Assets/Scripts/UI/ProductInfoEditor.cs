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
        
        [SerializeField] private TMP_InputField _name;
        [SerializeField] private TMP_InputField _description;
        [SerializeField] private TMP_InputField _price;
        [SerializeField] private Button         _save;
        [SerializeField] private Button         _cancel;

        private void OnEnable()
        {
            _save.onClick.AddListener(OnSave);
            _cancel.onClick.AddListener(OnCancel);
            _receiver = MessageBroker.Default.Receive<Product>().ObserveOnMainThread().Subscribe(OnEditProduct);
        }

        private void OnDisable()
        {
            _receiver.Dispose();
            _save.onClick.RemoveListener(OnSave);
            _cancel.onClick.RemoveListener(OnCancel);
        }

        private void OnSave()
        {
            // Send save event
            _productToEdit.ProductInfo.Description = _description.text;
            _productToEdit.ProductInfo.Price = decimal.Parse(_price.text);
            _productToEdit.ProductInfo.Name = _name.text;
        }

        private void OnCancel()
        {
            // Send cancel event
        }

        private void OnEditProduct(Product product)
        {
            _productToEdit = product;
        }
    }
}