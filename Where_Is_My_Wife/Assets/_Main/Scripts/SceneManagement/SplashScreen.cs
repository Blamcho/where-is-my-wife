using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Serialization;
using WhereIsMyWife.Managers;

namespace WhereIsMyWife.SceneManagement
{
    public class SplashScreen : MonoBehaviour
    {
        [SerializeField] private string _mainMenuSceneName = "MainMenu";
        
        private void Start()
        {
            SplashScreenAsync().Forget();
        }

        private async UniTaskVoid SplashScreenAsync()
        {
            // TODO: Add splash screen animation 
            
            LevelManager.Instance.LoadScene(_mainMenuSceneName);
        }
    }
}
