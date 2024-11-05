using UnityEngine;
using WhereIsMyWife.Managers;

public class AnimationEvents : MonoBehaviour
{
    public void EndPunch()
    {
        PlayerManager.Instance.PunchEnd?.Invoke();
    }
}
