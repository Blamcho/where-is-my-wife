using UnityEngine;
using UnityEngine.UI;
using WhereIsMyWife.Managers;

namespace WhereIsMyWife.SceneManagement
{
    [RequireComponent(typeof(Button))]
    public class ChangeSceneButton : MonoBehaviour
    {
        [SerializeField] private string _sceneName;

        private Button _button;
        
        private void Awake()
        {
            _button = gameObject.GetComponent<Button>();
            _button.onClick.AddListener(ChangeScene);
        }

        private void ChangeScene()
        {
            _button.interactable = false;
            LevelManager.Instance.LoadScene(_sceneName);
        }
    }
}

