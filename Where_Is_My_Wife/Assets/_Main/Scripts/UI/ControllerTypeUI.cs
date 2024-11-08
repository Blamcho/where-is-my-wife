using UnityEngine;
using UnityEngine.UI;
using WhereIsMyWife.Managers;

namespace WhereIsMyWife.UI
{
    public class ControllerTypeUI : MonoBehaviour
    {
        [SerializeField] private Sprite _keyboardSprite;
        [SerializeField] private Sprite _xboxSprite;
        [SerializeField] private Sprite _playstationSprite;
        [SerializeField] private Sprite _nintendoSprite;

        private Image _image;
        private SpriteRenderer _spriteRenderer;

        private void Awake()
        {
            _image = GetComponent<Image>();
        }

        void Start()
        {
            ChangeUIImage(InputEventManager.Instance.CurrentControllerType);
            
            InputEventManager.Instance.ChangeControllerTypeAction += ChangeUIImage;
        }

        private void OnDestroy()
        {
            InputEventManager.Instance.ChangeControllerTypeAction -= ChangeUIImage;
        }

        private void ChangeUIImage(ControllerType controllerType)
        {
            switch (controllerType)
            {
                case ControllerType.Keyboard:
                    _image.sprite = _keyboardSprite;
                    break;
                case ControllerType.Xbox:
                    _image.sprite = _xboxSprite;
                    break;
                case ControllerType.Playstation:
                    _image.sprite = _playstationSprite;
                    break;
                case ControllerType.Nintendo:
                    _image.sprite = _nintendoSprite;
                    break;
            }
        }
    }
}
