using UnityEngine;
using WhereIsMyWife.Managers;

namespace WhereIsMyWife.UI
{
    public class Credits : MonoBehaviour
    {
        [SerializeField] private string _mainMenuSceneName = "MainMenu";
        
        public float _speed = 250f;
        private float _endCreditsYPosition = 1080f;
        public RectTransform _rectTransform;

        public bool _hasEnded = false;
        
        void Start()
        {
            _rectTransform = GetComponent<RectTransform>();
        }

        void Update()
        {
            if (_hasEnded)
                return;
            
            _rectTransform.anchoredPosition += Vector2.up * (_speed * Time.deltaTime);
        
            if (_rectTransform.anchoredPosition.y > _endCreditsYPosition)
            {
                _hasEnded = true;
                LevelManager.Instance.LoadScene(_mainMenuSceneName);
            }
        }
    }
}

