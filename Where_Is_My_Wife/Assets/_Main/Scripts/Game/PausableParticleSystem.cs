using UnityEngine;
using WhereIsMyWife.Managers;

namespace WhereIsMyWife.Stage
{
    [RequireComponent(typeof(ParticleSystem))]
    public class PausableParticleSystem : MonoBehaviour
    {
        private ParticleSystem _particleSystem;
        private bool _shouldPlayOnResume = false;

        private void Awake()
        {
            _particleSystem = GetComponent<ParticleSystem>();
        }

        private void Start()
        {
            GameManager.Instance.PauseEvent += Pause;
            GameManager.Instance.ResumeEvent += Resume;
        }

        private void OnDestroy()
        {
            GameManager.Instance.PauseEvent -= Pause;
            GameManager.Instance.ResumeEvent -= Resume;
        }

        private void Pause()
        {
            if (_particleSystem.isPlaying)
            {
                _particleSystem.Pause();
                _shouldPlayOnResume = true;
            }
        }

        private void Resume()
        {
            if (_shouldPlayOnResume)
            {
                _particleSystem.Play();
                _shouldPlayOnResume = false;
            }
        }
    }
}
