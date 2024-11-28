using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using WhereIsMyWife.Managers;

namespace WhereIsMyWife.UI
{
    public class MenuManager : Singleton<MenuManager>
    {
        [SerializeField] private EventSystem _eventSystem;
        [SerializeField] private RectTransform _currentMenu;

        [SerializeField] private float _menuAnimationDuration = 0.2f;
        [SerializeField] private Ease _menuAnimationEase;

        [SerializeField] private GameObject[] _menuBackgrounds;
        [SerializeField] private GameObject _currentMenuBackground;
        
        private RectTransform _nextMenu;

        private Vector3 _rightPosition;
        private Vector3 _leftPosition;

        private void Start()
        {
            _rightPosition = _currentMenu.position;
            _leftPosition = _rightPosition;
            _leftPosition.x -= _currentMenu.rect.width;
            
            ChangeToLastPlayedStoryModeLevelBackground();
        }

        public async UniTaskVoid ChangeMenu(RectTransform nextMenu, MenuChangeAnimation changeAnimation, 
            Button nextButton)
        {
            _eventSystem.enabled = false;
            _nextMenu = nextMenu;
            _nextMenu.gameObject.SetActive(true);
            
            Vector3 finalPosition = Vector3.zero;
            RectTransform menuToMove = null;
            
            switch (changeAnimation)
            {
                case MenuChangeAnimation.MoveIn:
                    _nextMenu.position = _leftPosition;
                    finalPosition = _rightPosition;
                    menuToMove = _nextMenu;
                    break;
                case MenuChangeAnimation.MoveOut:
                    _nextMenu.position = _rightPosition;
                    finalPosition = _leftPosition;
                    menuToMove = _currentMenu;
                    ChangeToLastPlayedStoryModeLevelBackground();
                    break;
            }

            await menuToMove.DOMove(finalPosition, _menuAnimationDuration)
                .SetEase(_menuAnimationEase)
                .AsyncWaitForCompletion();
            
            _currentMenu.gameObject.SetActive(false);
            _currentMenu = _nextMenu;
            _eventSystem.enabled = true;
            nextButton.Select();
        }

        private void ChangeToLastPlayedStoryModeLevelBackground()
        {
            ChangeBackground(DataSaveManager.Instance.GetData<int>(DataSaveManager.LastPlayedStoryModeLevelIndexKey));
        }
        
        public void ChangeBackground(int levelNumber)
        {
            _currentMenuBackground.SetActive(false);
            _currentMenuBackground = _menuBackgrounds[levelNumber];
            _currentMenuBackground.SetActive(true);
        }
        
        public enum MenuChangeAnimation
        {
            MoveIn,
            MoveOut,
        }
    }
}
