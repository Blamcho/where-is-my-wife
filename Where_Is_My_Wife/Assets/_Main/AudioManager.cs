using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

namespace WhereIsMyWife.Managers
{
    public class AudioManager : Singleton<AudioManager>
    {
        [SerializeField] private MusicClip[] _music, _sfx;
        [SerializeField] private AudioSource _musicSource, _sfxSource;
        [SerializeField] private AudioMixer _mixer;
        [SerializeField] private float _fadeDuration = 1f;
        
        private bool _isFading = false;

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
            MusicClip s = Array.Find(_music, x => x.name == name);
            if (s == null)
            {
                Debug.Log("The clip has been found");
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
            MusicClip s = Array.Find(_sfx, x => x.name == name);
            if (s == null)
            {
                Debug.Log("The clip has been found");
            }
            else
            {
                _sfxSource.clip = s.Clip;
                _sfxSource.Play();
            }
        }

        public void FadeOutMusic(Action onComplete = null)
        {
            if (_isFading) return;

            _isFading = true;
            _musicSource.DOFade(0, _fadeDuration).OnComplete(() =>
            {
                _musicSource.Stop();
                _isFading = false;
                onComplete?.Invoke();
            });
        }

        public void FadeInMusic()
        {
            if (_isFading) return;

            _isFading = true;
            _musicSource.DOFade(1, _fadeDuration).OnComplete(() => _isFading = false);
        }

        public void ChangeSceneWithFadeOut(string sceneName)
        {
            FadeOutMusic(() => SceneManager.LoadScene(sceneName));
        }
        
        public void SetMixerVolume(float volume, string mixerChannel)
        {
            volume += 0.0001f; // Never reach to zero
            _mixer.SetFloat(mixerChannel, Mathf.Log10(volume) * 20);
        }
    }

    [System.Serializable]
    public class MusicClip
    {
        public string name;
        public AudioClip Clip;
    }
    
}
