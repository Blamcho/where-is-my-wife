using System;
using DG.Tweening;
using UnityEngine;

namespace WhereIsMyWife.Managers
{
    public class GameManager : Singleton<GameManager>
    {
        public event Action PauseEvent;
        public event Action ResumeEvent;

        public static bool IsFullscreen => DataSaveManager.Instance.GetData<bool>(DataSaveManager.FullscreenKey);

        public bool IsPaused { get; private set; }

        private float _timeScaleBeforePause = 1f;
        private float _originalFixedDeltaTime;

        protected override void Awake()
        {
            base.Awake();
            
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            
            _originalFixedDeltaTime = Time.fixedDeltaTime;
        }

        private void Start()
        {
            SetFullscreen(DataSaveManager.Instance.GetData<bool>(DataSaveManager.FullscreenKey));
        }

        public void Pause()
        {
            IsPaused = true;
            DOTween.PauseAll();
            PauseEvent?.Invoke();

            _timeScaleBeforePause = Time.timeScale;
            SetTimeScale(1f);
        }

        public void Resume()
        {
            IsPaused = false;
            DOTween.PlayAll();
            ResumeEvent?.Invoke();

            SetTimeScale(_timeScaleBeforePause);
        }

        public void SetTimeScale(float value)
        {
            Time.timeScale = value;
            Time.fixedDeltaTime = _originalFixedDeltaTime * value;
        }

        public void SetFullscreen(bool isFullscreen)
        {
            Screen.fullScreen = isFullscreen;
            DataSaveManager.Instance.SetData(DataSaveManager.FullscreenKey, isFullscreen);
        }
    }
}
