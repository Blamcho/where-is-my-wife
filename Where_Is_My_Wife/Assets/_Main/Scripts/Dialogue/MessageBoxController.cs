using DG.Tweening;
using TMPro;
using UnityEngine;
using WhereIsMyWife.UI;

namespace WhereIsMyWife.Controllers
{
    public class MessageBoxController : Singleton<MessageBoxController>
    {
        [SerializeField]
        private CanvasGroup _canvasGroup;

        [SerializeField]
        private LocalizedText _localizedText;

        [SerializeField]
        private GameObject _dashGameObject;

        [SerializeField]
        private GameObject _jumpGameObject;

        [SerializeField]
        private GameObject _punchGameObject;

        [SerializeField]
        private GameObject _hookGameObject;

        [SerializeField]
        private GameObject _player;

        private Tween _bubbleTween;
        private float _fadeTime = 0.220f;

        void Update()
        {
            transform.position = _player.transform.position;
        }

        public void AppearBubble(string text, MessageBoxButtonType messageBoxButtonTypeType)
        {
            _localizedText.ChangeText(text);
            SetButtonType(messageBoxButtonTypeType);

            _bubbleTween!.Kill();
            _bubbleTween = _canvasGroup.DOFade(1, _fadeTime);
        }

        public void DissappearBubble()
        {
            _bubbleTween!.Kill();
            _bubbleTween = _canvasGroup.DOFade(0, _fadeTime);
        }

        void SetButtonType(MessageBoxButtonType messageBoxButtonType)
        {
            _hookGameObject.SetActive(messageBoxButtonType == MessageBoxButtonType.HOOK);
            _dashGameObject.SetActive(messageBoxButtonType == MessageBoxButtonType.DASH);
            _jumpGameObject.SetActive(messageBoxButtonType == MessageBoxButtonType.JUMP);
            _punchGameObject.SetActive(messageBoxButtonType == MessageBoxButtonType.PUNCH);
        }
    }

    public enum MessageBoxButtonType
    {
        NONE,
        HOOK,
        DASH,
        JUMP,
        PUNCH,
    }
}
