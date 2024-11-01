using UnityEngine;

public class PunchingTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            foreach (IPunchable punchable in other.GetComponents<IPunchable>())
            {
                punchable.Punch();
            }
        }
    }
}
