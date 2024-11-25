using System;
using DG.Tweening;
using UnityEngine;

namespace WhereIsMyWife.Managers
{
    public class GameManager : Singleton<GameManager>
    {
        public event Action PauseEvent;
        public event Action ResumeEvent;
        
        public bool IsPaused { get; private set; }

        protected override void Awake()
        {
            base.Awake();
            
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        public void Pause()
        {
            IsPaused = true;
            DOTween.PauseAll();
            PauseEvent?.Invoke();
        }
        
        public void Resume()
        {
            IsPaused = false;
            DOTween.PlayAll();
            ResumeEvent?.Invoke();
        }
    }
}