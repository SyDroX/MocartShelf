using System;
using System.Collections;
using EventData;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class MessageHandler : MonoBehaviour
    {
        private IDisposable _receiver;
        private float       _messageHiddenY;
        
        [SerializeField] private RectTransform _messageRoot;
        [SerializeField] private Image         _messageBackground;
        [SerializeField] private TMP_Text      _messageText;
        [SerializeField] private Button        _closeButton;
        [SerializeField] private Color         _successColor;
        [SerializeField] private Color         _errorColor;
        [SerializeField] private float         _displayDurationSeconds = 1f;
        [SerializeField] private float         _moveTimeSeconds = 1f;
        [SerializeField] private float         _messageTargetY;

        private void Awake()
        {
            _messageHiddenY = _messageRoot.anchoredPosition.y;
        }
        
        private void OnEnable()
        {
            _closeButton.onClick.AddListener(OnCloseButtonClicked);
            _receiver = MessageBroker.Default.Receive<MessageEventArgs>().ObserveOnMainThread().Subscribe(OnMessage);
        }

        private void OnDisable()
        {
            _receiver.Dispose();
        }

        private IEnumerator HandleMessageDisplay(MessageType messageType)
        {
            yield return MoveMessage(_messageHiddenY, _messageTargetY);
            
            if (messageType != MessageType.Error)
            {
                yield return new WaitForSeconds(_displayDurationSeconds);
                
                StartCoroutine(MoveMessage(_messageTargetY, _messageHiddenY));
            }
        }

        private IEnumerator MoveMessage(float from, float to)
        {
            var time = 0f;

            while (time < _moveTimeSeconds)
            {
                time += Time.deltaTime;
                Vector2 position = _messageRoot.anchoredPosition;
                position.y                    = Mathf.Lerp(from, to, time / _moveTimeSeconds);
                _messageRoot.anchoredPosition = position;

                yield return null;
            }
        }

        private void OnCloseButtonClicked()
        {
            StartCoroutine(MoveMessage(_messageTargetY, _messageHiddenY));
        }

        private void OnMessage(MessageEventArgs args)
        {
           _messageText.text = args.Message;

           switch (args.MessageType)
           {
               case MessageType.Error:
                   _messageBackground.color = _errorColor;

                   break;
               case MessageType.Success:
                   _messageBackground.color = _successColor;

                   break;
               default:
                   _messageBackground.color = _errorColor;
                   const string error = " Unknown message type.";
                   Debug.LogError(error);
                   _messageText.text += error;
                   break;
           }

           StartCoroutine(HandleMessageDisplay(args.MessageType));
        }
    }
}