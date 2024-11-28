using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using WhereIsMyWife.Managers;
using WhereIsMyWife.UI;

namespace WhereIsMyWife.SceneManagement
{
    public class FreeplayButton : ChangeSceneButton
    {
        [SerializeField] private int _levelNumber;
        [SerializeField] private TextMeshProUGUI _hiddenText;
        
        private void Start()
        {
            if (!LevelHasBeenCleared())
            {
                _text.gameObject.SetActive(false);
                _hiddenText.gameObject.SetActive(true);
                _button.interactable = false;
            }
        }

        private bool LevelHasBeenCleared()
        {
            return DataSaveManager.Instance.GetData<int>(DataSaveManager.LastUnlockedLevelNumberKey) > _levelNumber;
        }

        protected override void ChangeScene()
        {
            LevelManager.Instance.SetStoryMode(false);
            base.ChangeScene();
        }

        public override void OnSelect(BaseEventData eventData)
        {
            base.OnSelect(eventData);
            MenuManager.Instance.ChangeBackground(_levelNumber);
        }
    }
}

