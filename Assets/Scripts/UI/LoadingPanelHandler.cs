using System;
using EventData;
using UniRx;
using UnityEngine;

namespace UI
{
    public class LoadingPanelHandler : MonoBehaviour
    {
        private IDisposable _receiver;

        [SerializeField] private GameObject _loadingPanelRoot;

        private void OnEnable()
        {
            _receiver = MessageBroker.Default.Receive<LoadingPanelEventArgs>().ObserveOnMainThread().Subscribe(OnLoading);
        }

        private void OnDisable()
        {
            _receiver.Dispose();
        }

        private void OnLoading(LoadingPanelEventArgs args)
        {
            _loadingPanelRoot.SetActive(args.Enabled);
        }
    }
}