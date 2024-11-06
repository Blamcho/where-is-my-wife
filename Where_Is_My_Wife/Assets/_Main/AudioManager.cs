using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace WhereIsMyWife.Managers
{
    public class AudioManager : Singleton<AudioManager>
    {
        public Sounds[] _music, _sfx;
        public AudioSource _musicSource, _sfxSource;

        public float fadeDuration = 1f;
        private bool isFading = false;

        private void Start()
        {
            
            PlayMusic(SceneManager.GetActiveScene().name, true);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            PlayMusic(scene.name, true);
        }

        public void PlayMusic(string name, bool fadeIn = false)
        {
            Sounds s = Array.Find(_music, x => x.name == name);
            if (s == null)
            {
                Debug.Log("No se encontró el clip");
                return;
            }

            _musicSource.clip = s.Clip;

            if (fadeIn)
            {
                _musicSource.volume = 0;
                _musicSource.Play();
                FadeInMusic();
            }
            else
            {
                _musicSource.Play();
            }
        }

        public void PlaySFX(string name)
        {
            Sounds s = Array.Find(_sfx, x => x.name == name);
            if (s == null)
            {
                Debug.Log("No se encontró el clip");
            }
            else
            {
                _sfxSource.clip = s.Clip;
                _sfxSource.Play();
            }
        }

        public void FadeOutMusic(Action onComplete = null)
        {
            if (isFading) return;

            isFading = true;
            _musicSource.DOFade(0, fadeDuration).OnComplete(() =>
            {
                _musicSource.Stop();
                isFading = false;
                onComplete?.Invoke();
            });
        }

        public void FadeInMusic()
        {
            if (isFading) return;

            isFading = true;
            _musicSource.DOFade(1, fadeDuration).OnComplete(() => isFading = false);
        }

        public void ChangeSceneWithFadeOut(string sceneName)
        {
            
            FadeOutMusic(() => SceneManager.LoadScene(sceneName));
        }
        public void SetVolume(float volume, bool isMusic)
        {
            if (isMusic)
                _musicSource.volume = volume;
            else
                _sfxSource.volume = volume;
        }
    }

    [System.Serializable]
    public class Sounds
    {
        public string name;
        public AudioClip Clip;
    }
    
}
