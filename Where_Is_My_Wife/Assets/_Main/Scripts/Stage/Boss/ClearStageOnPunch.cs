using UnityEngine;
using WhereIsMyWife.Managers;

public class ClearStageOnPunch : MonoBehaviour, IPunchable
{
    [SerializeField] private int _stageToClear;
    
    public void Punch()
    {
        BossManager.Instance.ClearStage(_stageToClear);
    }
}
