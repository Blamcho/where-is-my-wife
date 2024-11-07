using UnityEngine;
using UnityEngine.Serialization;

namespace WhereIsMyWife.LavaRiseTriggers
{
    public class LavaTrigger : MonoBehaviour
    {
        [SerializeField] private LavaRise _lava;
        [SerializeField] private LavaAction _action = LavaAction.Rise;

        private enum LavaAction
        {
            Rise,
            PositionBelowCamera,
        }
        
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                switch (_action)
                {
                    case LavaAction.Rise:
                        _lava.StartRising();
                        break;
                    case LavaAction.PositionBelowCamera:
                        _lava.PositionBelowCamera();
                        break;
                }
            }
        }
    }
}
