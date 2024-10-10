using UnityEngine;
using UnityEngine.UI;
using WhereIsMyWife.Managers;

namespace WhereIsMyWife.SceneManagement
{
    [RequireComponent(typeof(Button))]
    public class ChangeSceneButton : MonoBehaviour
    {
        [SerializeField] private string _sceneName;

        private void Awake()
        {
            gameObject.GetComponent<Button>().onClick.AddListener(ChangeScene);
        }

        private void ChangeScene()
        {
            LevelManager.Instance.LoadScene(_sceneName);
            Destroy(gameObject);
        }
    }
}

