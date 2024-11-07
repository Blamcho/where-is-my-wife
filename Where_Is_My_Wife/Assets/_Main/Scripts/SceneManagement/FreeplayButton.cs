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
            if (DataSaveManager.Instance.GetData<int>(DataSaveManager.LastUnlockedLevelKey) < _levelNumber)
            {
                _text.gameObject.SetActive(false);
                _hiddenText.gameObject.SetActive(true);
                _button.interactable = false;
            }
        }
    }
}

