using UnityEngine;
using WhereIsMyWife.Managers;

public class BossStageTrigger : MonoBehaviour
{
    //TODO: Add different cases if boss action gets added
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            BossManager.Instance.StartFiring();
        }
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            BossManager.Instance.StopFiring();
        }
    }
}
