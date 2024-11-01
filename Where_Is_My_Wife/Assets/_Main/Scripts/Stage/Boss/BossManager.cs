using System;
using UnityEngine;

namespace WhereIsMyWife.Managers
{
    public class BossManager : Singleton<BossManager>
    {
        [SerializeField] private int _bossStagesAmount;
        [SerializeField] private string _storyEndSceneName;
        
        public event Action<int> GoToNextStageEvent;
        
        public event Action StartFiringEvent;
        public event Action StopFiringEvent;
        public event Action StartSwayingEvent;
        public event Action StopSwayingEvent;

        public void ClearStage(int clearedStageNumber)
        {
            Debug.Log($"Cleared stage {clearedStageNumber}");
            
            if (clearedStageNumber >= _bossStagesAmount)
            {
                Debug.Log("Boss Defeated");
                Die();
            }
            else
            {
                Debug.Log($"Advancing to stage {clearedStageNumber + 1}");
                GoToNextStageEvent?.Invoke(clearedStageNumber + 1);
            }
        }
        
        public void StartFiring()
        {
            StartFiringEvent?.Invoke();
        }

        public void StopFiring()
        {
            StopFiringEvent?.Invoke();
        }

        public void StartSwaying()
        {
            StartSwayingEvent?.Invoke();
        }

        public void StopSwaying()
        {
            StopSwayingEvent?.Invoke();
        }
        
        private void Die()
        {
            // TODO: Add or call death/defeat animation
            LevelManager.Instance.LoadScene(_storyEndSceneName);
        }
        
        public enum BossAction
        {
            Swaying,
            Firing,
        }
    }
}
