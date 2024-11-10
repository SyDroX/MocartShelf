using System;
using System.Globalization;
using Entities;
using TMPro;
using UniRx;
using UnityEngine;

namespace UI
{
    public class ProductInfoDisplayer : MonoBehaviour
    {
        private IDisposable _receiver;

        [SerializeField] private TMP_Text _name;
        [SerializeField] private TMP_Text _price;
        [SerializeField] private TMP_Text _description;

        private void OnEnable()
        {
            _receiver = MessageBroker.Default.Receive<ProductInfo>().ObserveOnMainThread().Subscribe(OnDisplayProductInfo);
        }

        private void OnDisable()
        {
            _receiver.Dispose();
        }
        
        private void OnDisplayProductInfo(ProductInfo productInfo)
        {
            _name.text        = productInfo.Name;
            _description.text = productInfo.Description;
            _price.text       = productInfo.Price.ToString(CultureInfo.InvariantCulture);
        }
    }
}