using EventData;
using UniRx;
using UnityEngine;

namespace UI
{
    public class ScreenResolutionListener : MonoBehaviour
    {
        private float       _width;
        private float       _height;
        private bool        _isPortrait;
        
        private void Start()
        {
            UpdateCurrentSizes();

            if (_height > _width)
            {
                _isPortrait = true;
            }
        }

        private void LateUpdate()
        {
            if (!Mathf.Approximately(Screen.width, _width) || !Mathf.Approximately(Screen.height, _height))
            {
                UpdateCurrentSizes();
                bool wasPortrait = _isPortrait;
                _isPortrait = _height > _width;

                if (wasPortrait != _isPortrait)
                {
                    MessageBroker.Default.Publish(new OrientationChangedEventArgs
                    {
                        Width  = _width,
                        Height = _height
                    });
                }
            }
        }
        
        private void UpdateCurrentSizes()
        {
            _width  = Screen.width;
            _height = Screen.height;
        }
    }
}