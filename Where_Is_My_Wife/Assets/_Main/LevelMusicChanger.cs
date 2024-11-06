using UnityEngine;
using UnityEngine.SceneManagement;
using WhereIsMyWife.Managers;
public class LevelMusicChanger : MonoBehaviour
{
    void Start()
    {
        AudioManager.Instance.PlayMusic(SceneManager.GetActiveScene().name,true);
    }
    public void ChangeSceneWithMusic(string sceneName)
    {
        AudioManager.Instance.ChangeSceneWithFadeOut(sceneName);
    }
}
