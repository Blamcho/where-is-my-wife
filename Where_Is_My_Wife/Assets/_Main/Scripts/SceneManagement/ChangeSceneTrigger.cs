using UnityEngine;
using WhereIsMyWife.Managers;

namespace WhereIsMyWife.SceneManagement
{
    public class ChangeSceneTrigger : MonoBehaviour
    {
        [SerializeField] private string _sceneName;
        private bool _hasBeenTriggered = false;
    
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player") && !_hasBeenTriggered)
            {
                _hasBeenTriggered = true;
                LevelManager.Instance.LoadScene(_sceneName);
            }
        }
    }
}
