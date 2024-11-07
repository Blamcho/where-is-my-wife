using UnityEngine;
using UnityEngine.SceneManagement;
using WhereIsMyWife.Managers;
public class LevelMusicChanger : MonoBehaviour
{
    void Start()
    {
        AudioManager.Instance.PlayMusic(SceneManager.GetActiveScene().name,true);
    }
}
