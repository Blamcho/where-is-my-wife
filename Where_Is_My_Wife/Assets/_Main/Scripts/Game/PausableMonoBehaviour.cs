using System;
using UnityEngine;
using WhereIsMyWife.Managers;

public class PausableMonoBehaviour : MonoBehaviour
{
    private bool _isPaused;
    
    protected virtual void Start()
    {
        _isPaused = GameManager.Instance.IsPaused;
        
        if (_isPaused)
        {
            Pause();
        }
        else
        {
            Resume();
        }
        
        GameManager.Instance.PauseEvent += PauseBehaviour;
        GameManager.Instance.ResumeEvent += ResumeBehaviour;
    }

    protected virtual void OnDestroy()
    {
        GameManager.Instance.PauseEvent -= PauseBehaviour;
        GameManager.Instance.ResumeEvent -= ResumeBehaviour;
    }

    protected virtual void Update()
    {
        if (_isPaused) return;
    }

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
