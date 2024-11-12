using System;
using System.Collections;
using EventData;
using UI.Product;
using UniRx;
using UnityEngine;

namespace UI
{
    public class CameraRotator : MonoBehaviour
    {
        private IDisposable _receiver;

        [SerializeField] private Transform _cameraTransform;
        [SerializeField] private float     _rotationTimeSeconds = 0.35f;

        private void OnEnable()
        {
            _receiver = MessageBroker.Default.Receive<SelectedProductPositionEventArgs>()
                                     .ObserveOnMainThread()
                                     .Subscribe(OnSelectedPositionChanged);
        }

        private void OnDisable()
        {
            _receiver.Dispose();
        }

        private void OnSelectedPositionChanged(SelectedProductPositionEventArgs args)
        {
            StartCoroutine(RotateCameraToTarget(args.Position));
        }

        private IEnumerator RotateCameraToTarget(Vector3 targetPosition)
        {
            Vector3    direction       = targetPosition - _cameraTransform.position;
            var        targetDirection = new Vector3(direction.x, 0, direction.z);
            Quaternion targetRotation  = Quaternion.LookRotation(targetDirection);
            Quaternion initialRotation = _cameraTransform.rotation;
            var        elapsedTime     = 0f;

            while (elapsedTime < _rotationTimeSeconds)
            {
                _cameraTransform.rotation =  Quaternion.Slerp(initialRotation, targetRotation, elapsedTime / _rotationTimeSeconds);
                elapsedTime               += Time.deltaTime;

                yield return null;
            }

            _cameraTransform.rotation = targetRotation;
        }
    }
}