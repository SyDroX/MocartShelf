using System;
using UnityEngine;

namespace UI
{
    public class CameraPositionAdjuster : MonoBehaviour
    {
        private float       _width;
        private float       _height;
        private IDisposable _receiver;

        [SerializeField] private Vector3   _landscapePosition;
        [SerializeField] private Vector3   _portraitPosition;
        [SerializeField] private Transform _cameraTransform;

        private void Start()
        {
            SetCameraPosition();
        }

        private void LateUpdate()
        {
            if (!Mathf.Approximately(Screen.width, _width) || !Mathf.Approximately(Screen.height, _height))
            {
                SetCameraPosition();
            }
        }

        private void SetCameraPosition()
        {
            _width  = Screen.width;
            _height = Screen.height;

            _cameraTransform.position = _width > _height ? _landscapePosition : _portraitPosition;
        }
    }
}