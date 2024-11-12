using System;
using System.Collections;
using EventData;
using UniRx;
using UnityEngine;

namespace UI
{
    public class CameraMover : MonoBehaviour
    {
        private IDisposable _receiver;
        private IDisposable _receiver2;

        [SerializeField] private Transform _cameraTransform;
        [SerializeField] private float     _rotationTimeSeconds = 0.35f;
        [SerializeField] private Vector3   _landscapePosition;
        [SerializeField] private Vector3   _portraitPosition;
        
        private void OnEnable()
        {
            _receiver = MessageBroker.Default.Receive<SelectedProductPositionEventArgs>()
                                     .ObserveOnMainThread()
                                     .Subscribe(OnSelectedPositionChanged);
            _receiver2 = MessageBroker.Default.Receive<OrientationChangedEventArgs>()
                                     .ObserveOnMainThread()
                                     .Subscribe(OnOrientationChanged);
        }

        private void Start()
        {
            SetCameraPosition(Screen.width, Screen.height);
        }

        private void OnDisable()
        {
            _receiver.Dispose();
            _receiver2.Dispose();
        }

        private void OnSelectedPositionChanged(SelectedProductPositionEventArgs args)
        {
            //StartCoroutine(RotateCameraToTarget(args.Position));
            StartCoroutine(MoveCameraToTarget(args.Position.x));
        }

        private void OnOrientationChanged(OrientationChangedEventArgs args)
        {
            SetCameraPosition(args.Width,args.Height);
        }

        private void SetCameraPosition(float width, float height)
        {
            Vector3 newPosition = width > height ? _landscapePosition : _portraitPosition;
            newPosition.x             = _cameraTransform.position.x;
            _cameraTransform.position = newPosition;
        }
        
        private IEnumerator RotateCameraToTarget(Vector3 targetPosition)
        {
            Vector3    direction       = targetPosition - _cameraTransform.position;
            var        targetDirection = new Vector3(direction.x, 0, direction.z);
            Quaternion initialRotation = _cameraTransform.rotation;
            Quaternion targetRotation  = Quaternion.LookRotation(targetDirection);
            targetRotation = Quaternion.Euler(initialRotation.eulerAngles.x, targetRotation.eulerAngles.y, targetRotation.eulerAngles.z);
            var elapsedTime = 0f;

            while (elapsedTime < _rotationTimeSeconds)
            {
                _cameraTransform.rotation =  Quaternion.Slerp(initialRotation, targetRotation, elapsedTime / _rotationTimeSeconds);
                elapsedTime               += Time.deltaTime;

                yield return null;
            }

            _cameraTransform.rotation = targetRotation;
        }
        
        private IEnumerator MoveCameraToTarget(float targetX)
        {
            Vector3 initialPosition = _cameraTransform.position;
            var targetPosition  = new Vector3(targetX, initialPosition.y, initialPosition.z);
            var     elapsedTime     = 0f;
            
            while (elapsedTime < _rotationTimeSeconds)
            {
                _cameraTransform.position =  Vector3.Lerp(initialPosition, targetPosition, elapsedTime / _rotationTimeSeconds);
                elapsedTime        += Time.deltaTime;
                
                yield return null;
            }

            _cameraTransform.position = targetPosition;
        }
    }
}