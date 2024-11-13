using System;
using EventData;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class CanvasOrientationFlipper : MonoBehaviour
    {
        private IDisposable _receiver;

        [SerializeField] private CanvasScaler _canvasScaler;

        private void OnEnable()
        {
            _receiver = MessageBroker.Default.Receive<OrientationChangedEventArgs>()
                                     .ObserveOnMainThread()
                                     .Subscribe(OnOrientationChanged);
        }

        private void Start()
        {
            if (Screen.width < Screen.height)
            {
                Flip();
            }
        }

        private void OnDisable()
        {
            _receiver.Dispose();
        }

        private void OnOrientationChanged(OrientationChangedEventArgs _)
        {
            Flip();
        }

        private void Flip()
        {
            Vector2 currentResolution = _canvasScaler.referenceResolution;
            _canvasScaler.referenceResolution = new Vector2(currentResolution.y, currentResolution.x);
        }
    }
}