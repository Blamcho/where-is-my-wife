using UnityEngine;
using UnityEngine.Serialization;

namespace WhereIsMyWife.LavaRiseTriggers
{
    public class LavaTrigger : MonoBehaviour
    {
        [SerializeField] private LavaRise _lava;
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                _lava.StartRising();
            }
        }
    }
}
