using UnityEngine;
using WhereIsMyWife.Managers;

namespace WhereIsMyWife.Game
{
    public class PausableMonoBehaviour : MonoBehaviour
    {
        private bool _isPaused;
    
        protected virtual void Start()
        {
            _isPaused = GameManager.Instance.IsPaused;
        
            GameManager.Instance.PauseEvent += PauseBehaviour;
            GameManager.Instance.ResumeEvent += ResumeBehaviour;
        }

        protected virtual void OnDestroy()
        {
            GameManager.Instance.PauseEvent -= PauseBehaviour;
            GameManager.Instance.ResumeEvent -= ResumeBehaviour;
        }

        private void Update()
        {
            if (_isPaused) return;
        
            OnUpdate();
        }

        protected virtual void OnUpdate() { }

        private void PauseBehaviour()
        {
            _isPaused = true;
            Pause();
        }
    
        private void ResumeBehaviour()
        {
            _isPaused = false;
            Resume();
        }

        protected virtual void Pause() { }

        protected virtual void Resume() { }
    }
}
