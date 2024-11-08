using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace WhereIsMyWife.SceneManagement
{
    public class StoryBoardAnimation : MonoBehaviour
    {
        [SerializeField] private Button _nextSceneButton;
        [SerializeField] private Image[] comicImages; 
        [SerializeField] private float fadeDuration = 1f; 
        [SerializeField] private float intervalBetweenImages = 2f;

        private void Start()
        {
            StoryBoardAnimationAsync().Forget();
        }

        private async UniTaskVoid StoryBoardAnimationAsync()
        {
            _nextSceneButton.interactable = false;
            _nextSceneButton.gameObject.SetActive(false);
            foreach (var image in comicImages)
            {
                image.color = new Color(1, 1, 1, 0);
            }
            
            for (int i = 0; i < comicImages.Length; i++)
            {
                comicImages[i].DOFade(1, fadeDuration); 
                await UniTask.Delay((int)((fadeDuration + intervalBetweenImages) * 1000));
            }
            _nextSceneButton.gameObject.SetActive(true);
            _nextSceneButton.interactable = true;
            _nextSceneButton.Select();
        }
    }
}
