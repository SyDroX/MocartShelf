using System;
using System.Globalization;
using EventData;
using TMPro;
using UniRx;
using UnityEngine;

namespace UI.Product
{
    public class ProductInfoDisplayer : MonoBehaviour
    {
        private IDisposable _receiver;

        [SerializeField] private TMP_Text _nameText;
        [SerializeField] private TMP_Text _priceText;
        [SerializeField] private TMP_Text _descriptionText;

        private void OnEnable()
        {
            _receiver = MessageBroker.Default.Receive<DisplayProductInfoEventArgs>().ObserveOnMainThread().Subscribe(OnDisplayProductInfo);
        }

        private void OnDisable()
        {
            _receiver.Dispose();
        }
        
        private void OnDisplayProductInfo(DisplayProductInfoEventArgs args)
        {
            _nameText.text        = args.ProductInfo.Name;
            _descriptionText.text = args.ProductInfo.Description;
            _priceText.text       = args.ProductInfo.Price.ToString(CultureInfo.InvariantCulture);
        }
    }
}