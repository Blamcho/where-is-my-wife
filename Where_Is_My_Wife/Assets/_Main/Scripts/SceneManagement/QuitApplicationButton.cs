using UnityEngine;
using UnityEngine.UI;

namespace WhereIsMyWife.SceneManagement
{
    [RequireComponent(typeof(Button))]
    public class QuitApplicationButton : MonoBehaviour
    {
        private void Awake()
        {
            gameObject.GetComponent<Button>().onClick.AddListener(QuitApplication);
        }

        private void QuitApplication()
        {
            Application.Quit();
        }
    }
}
