using UnityEngine;
using UnityEngine.SceneManagement;

namespace WhereIsMyWife.UI
{
    public class MenuController : MonoBehaviour
    {
        public void Play()
        {
            SceneManager.LoadScene("Partial1");
        }

        public void ExitGame()
        {
            Application.Quit();
        }
    }
}
