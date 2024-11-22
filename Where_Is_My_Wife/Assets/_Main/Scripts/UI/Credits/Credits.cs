using UnityEngine;
using WhereIsMyWife.Game;
using WhereIsMyWife.Managers;

namespace WhereIsMyWife.UI
{
    public class Credits : PausableMonoBehaviour
    {
        public float _speed = 250f;
        private float _endCreditsYPosition = 1080f;
        public RectTransform _rectTransform;

        public bool _hasEnded = false;

        protected override void Start()
        {
            _rectTransform = GetComponent<RectTransform>();
        }

        protected override void OnUpdate()
        {
            if (_hasEnded)
                return;
            
            _rectTransform.anchoredPosition += Vector2.up * (_speed * Time.deltaTime);
        
            if (_rectTransform.anchoredPosition.y > _endCreditsYPosition)
            {
                _hasEnded = true;
                LevelManager.Instance.LoadScene(LevelManager.MainMenuSceneName);
            }
        }
    }
}

