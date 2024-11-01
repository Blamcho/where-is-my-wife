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

        private void Die()
        {
            // TODO: Add or call death/defeat animation
            LevelManager.Instance.LoadScene(_storyEndSceneName);
        }
    }
}
