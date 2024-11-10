using System;
using System.Globalization;
using Entities;
using TMPro;
using UniRx;
using UnityEngine;

public class ProductInfoDisplayer : MonoBehaviour
{
    private IDisposable _receiver;

    [SerializeField] private TMP_Text _nameText;
    [SerializeField] private TMP_Text _descriptionText;
    [SerializeField] private TMP_Text _priceText;

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
        _nameText.text        = productInfo.Name;
        _descriptionText.text = productInfo.Description;
        _priceText.text       = productInfo.Price.ToString(CultureInfo.InvariantCulture);
    }
}