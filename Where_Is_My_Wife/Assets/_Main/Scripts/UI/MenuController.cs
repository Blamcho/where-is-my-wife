using UnityEngine;
using UnityEngine.SceneManagement;

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
