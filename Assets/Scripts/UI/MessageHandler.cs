using System;
using System.Collections;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public enum MessageType
    {
        Error = 0,
        Success
    }
    
    public class MessageEventArgs
    {
        public MessageType MessageType;
        public string Message;
    }
    
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
           _messageBackground.color = args.MessageType switch
           {
               MessageType.Error => _errorColor,
               MessageType.Success => _successColor, 
               _ => throw new ArgumentOutOfRangeException()
           };

           StartCoroutine(HandleMessageDisplay(args.MessageType));
        }
    }
}