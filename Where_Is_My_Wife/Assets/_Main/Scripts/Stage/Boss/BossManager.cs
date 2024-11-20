using System;
using UnityEngine;

namespace WhereIsMyWife.Managers
{
    public class BossManager : Singleton<BossManager>
    {
        [SerializeField] private int _bossStagesAmount;
        
        public event Action<int> GoToNextStageEvent;
        public event Action StartFiringEvent;
        public event Action StopFiringEvent;
        public event Action StartSwayingEvent;
        public event Action StopSwayingEvent;
        public event Action StartFinalPhaseEvent;
        public event Action StartFinalAttackEvent;
        public event Action StopFinalAttackEvent;
        public event Action TakeDamageEvent;
        public event Action DieEvent;

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
            DieEvent?.Invoke();
        }
        
        public enum BossAction
        {
            Swaying,
            Firing,
            FinalAttack,
        }

        public void TakeDamage()
        {
            TakeDamageEvent?.Invoke();
        }
    }
}
