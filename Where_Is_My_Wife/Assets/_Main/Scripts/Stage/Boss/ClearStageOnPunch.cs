using UnityEngine;
using WhereIsMyWife.Managers;

namespace WhereIsMyWife.Stage
{
    public class ClearStageOnPunch : MonoBehaviour, IPunchable
    {
        [SerializeField] private int _stageToClear;
    
        public void Punch()
        {
            BossManager.Instance.ClearStage(_stageToClear);
        }
    }
}
