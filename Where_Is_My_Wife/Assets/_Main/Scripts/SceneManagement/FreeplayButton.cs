using System;
using TMPro;
using UnityEngine;
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
            if (DataSaveManager.Instance.GetData<int>(DataSaveManager.LastUnlockedLevelNumberKey) < _levelNumber)
            {
                _text.gameObject.SetActive(false);
                _hiddenText.gameObject.SetActive(true);
                _button.interactable = false;
            }
        }
        
        protected override void ChangeScene()
        {
            LevelManager.Instance.SetStoryMode(false);
            base.ChangeScene();
        }
    }
}

