using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using WhereIsMyWife.Managers;

namespace WhereIsMyWife.SceneManagement
{
    public class StoryBoardAnimation : MonoBehaviour
    {
        [SerializeField] private Button _nextSceneButton;
        
        private void Start()
        {
            StoryBoardAnimationAsync().Forget();
        }

        private async UniTaskVoid StoryBoardAnimationAsync()
        {
            _nextSceneButton.interactable = false;
            
            // TODO: Add story board animation and delete following line
            await UniTask.Delay(TimeSpan.FromSeconds(0.5f));
            
            _nextSceneButton.interactable = true;
            _nextSceneButton.Select();
        }
    }
}
