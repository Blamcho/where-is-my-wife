using System;
using UnityEngine;

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
        public event Action StartFinalPhaseEvent;
        public event Action StartFinalAttackEvent;
        public event Action StopFinalAttackEvent;

        public void ClearStage(int clearedStageNumber)
        {
            if (clearedStageNumber >= _bossStagesAmount)
            {
                Die();
            }
            else
            {
                if (clearedStageNumber + 1 == _bossStagesAmount)
                {
                    StartFinalPhaseEvent?.Invoke();
                }
                
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

        public void StartFinalAttack()
        {
            StartFinalAttackEvent?.Invoke();
        }
        
        public void StopFinalAttack()
        {
            StopFinalAttackEvent?.Invoke();
        }
        
        private void Die()
        {
            // TODO: Add or call death/defeat animation
            
            if (LevelManager.Instance.IsInStoryMode)
            {
                DataSaveManager.Instance.SetNextLevelParameters(_currentLevelNumber, _currentLevelInitialScene, true);
            }
            
            LevelManager.Instance.LoadScene(_storyEndSceneName);
        }
        
        public enum BossAction
        {
            Swaying,
            Firing,
            FinalAttack,
        }
    }
}
