using UnityEngine;
using UnityEngine.Serialization;

namespace WhereIsMyWife.lava_ballvertical_motion
{
    public class LavaBallMovement : MonoBehaviour
    { 
        [SerializeField] private float _speed = 2f;
        [SerializeField] private float _height = 3f;
        private Vector3 _startPos;

        void Start()
        {
            _startPos = transform.position;
        }

        void Update()
        {
            float newY = Mathf.PingPong(Time.time * _speed, _height);
            transform.position = new Vector3(_startPos.x, _startPos.y + newY, _startPos.z);
        }
    }
}
