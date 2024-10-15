using UnityEngine;
using WhereIsMyWife.Managers;
using WhereIsMyWife.UI;

namespace WhereIsMyWife.SceneManagement
{
    public class ChangeSceneButton : MenuButton
    {
        [SerializeField] private string _sceneName;

        protected override void Awake()
        {
            base.Awake();
            _button.onClick.AddListener(ChangeScene);
        }

        private void ChangeScene()
        {
            _button.interactable = false;
            LevelManager.Instance.LoadScene(_sceneName);
        }
    }
}

