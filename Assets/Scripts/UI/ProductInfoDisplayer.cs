using System;
using System.Globalization;
using Entities;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class ProductInfoDisplayer : MonoBehaviour
    {
        private IDisposable _receiver;

        [SerializeField] private TMP_Text _name;
        [SerializeField] private TMP_Text _description;
        [SerializeField] private TMP_Text _price;
        [SerializeField] private Button   _edit;
        
        private void OnEnable()
        {
            _edit.onClick.AddListener(OnEditClick);
            _receiver = MessageBroker.Default.Receive<ProductInfo>().ObserveOnMainThread().Subscribe(OnDisplayProductInfo);
        }

        private void OnDisable()
        {
            _edit.onClick.RemoveListener(OnEditClick);
            _receiver.Dispose();
        }

        private void OnEditClick()
        {
            // Send edit event
        }

        private void OnDisplayProductInfo(ProductInfo productInfo)
        {
            _name.text        = productInfo.Name;
            _description.text = productInfo.Description;
            _price.text       = productInfo.Price.ToString(CultureInfo.InvariantCulture);
        }
    }
}