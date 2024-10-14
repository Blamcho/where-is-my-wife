using UnityEngine;
using WhereIsMyWife.Managers;

public class PunchingEvents : MonoBehaviour
{
    public void EndPunch()
    {
        PlayerManager.Instance.PunchEnd?.Invoke();
    }
}
