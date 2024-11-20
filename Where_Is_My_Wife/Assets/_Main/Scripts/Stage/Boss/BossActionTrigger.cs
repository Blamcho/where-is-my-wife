using UnityEngine;
using WhereIsMyWife.Managers;

namespace WhereIsMyWife.Stage
{
    public class BossActionTrigger : MonoBehaviour
    {
        [SerializeField] private BossManager.BossAction _bossAction;

        private BossManager _bossManager;
    
        private void Start()
        {
            _bossManager = BossManager.Instance;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                switch (_bossAction)
                {
                    case BossManager.BossAction.Firing:
                        _bossManager.StartFiring();
                        break;
                    case BossManager.BossAction.Swaying:
                        _bossManager.StartSwaying();
                        break;
                    case BossManager.BossAction.FinalAttack:
                        _bossManager.StartFinalAttack();
                        break;
                }
            }
        }
    
        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                switch (_bossAction)
                {
                    case BossManager.BossAction.Firing:
                        _bossManager.StopFiring();
                        break;
                    case BossManager.BossAction.Swaying:
                        _bossManager.StopSwaying();
                        break;
                    case BossManager.BossAction.FinalAttack:
                        _bossManager.StopFinalAttack();
                        break;
                }
            }
        }
    }
}
