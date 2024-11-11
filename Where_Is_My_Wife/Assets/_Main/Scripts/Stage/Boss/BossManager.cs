using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace WhereIsMyWife.Managers
{
    public class BossManager : Singleton<BossManager>
    {
        [SerializeField] private int _bossStagesAmount;
        [SerializeField] private string _storyEndSceneName = "Story5";
        
        [SerializeField] private int _currentLevelNumber = 5;
        [SerializeField] private string _currentLevelInitialScene = "Story4";
        
        public event Action<int> GoToNextStageEvent;
        
        public event Action StartFiringEvent;
        public event Action StopFiringEvent;
        public event Action StartSwayingEvent;
        public event Action StopSwayingEvent;

        public void ClearStage(int clearedStageNumber)
        {
            if (clearedStageNumber >= _bossStagesAmount)
            {
                Die();
            }
            else
            {
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
            
            DataSaveManager.Instance.SetNextLevelParameters(_currentLevelNumber, _currentLevelInitialScene);
            LevelManager.Instance.LoadScene(_storyEndSceneName);
        }
        
        public enum BossAction
        {
            Swaying,
            Firing,
        }
    }
}
