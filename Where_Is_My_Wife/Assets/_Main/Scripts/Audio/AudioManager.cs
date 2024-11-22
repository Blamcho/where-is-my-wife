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

        private Sequence _fadeSequence;
        
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
            MusicClip musicClip = Array.Find(_music, x => x.name == name);
            if (musicClip == null)
            {
                Debug.Log("The clip wasn't found");
                return;
            }
            
            _fadeSequence?.Kill();
            _fadeSequence = DOTween.Sequence();
            if (fadeIn) _fadeSequence.Append(_musicSource.DOFade(0, _fadeDuration));
            _fadeSequence.AppendCallback(() =>
            { 
                _musicSource.clip = musicClip.Clip;
                _musicSource.Play();
            });
            if (fadeIn) _fadeSequence.Append(_musicSource.DOFade(1, _fadeDuration));
        }

        public void StopMusic(bool fadeIn = false)
        {
            _fadeSequence?.Kill();
            _fadeSequence = DOTween.Sequence();
            if (fadeIn) _fadeSequence.Append(_musicSource.DOFade(0, _fadeDuration));
            _fadeSequence.AppendCallback(() =>
            {
                _musicSource.Stop(); 
                _musicSource.volume = 1;
            });
        }
        
        public void PlaySFX(string name)
        {
            MusicClip s = Array.Find(_sfx, x => x.name == name);
            if (s == null)
            {
                Debug.Log("The clip wasn't found");
            }
            else
            {
                _sfxSource.clip = s.Clip;
                _sfxSource.PlayOneShot(s.Clip);
            }
        }
        
        public void SetMixerVolume(float volume, string mixerChannel)
        {
            volume += 0.0001f; // Never reach to zero
            if (mixerChannel == "Music")
            {
                _mixer.SetFloat(mixerChannel, Mathf.Log10((volume/2)) * 20);
            }
            else
            {
                _mixer.SetFloat(mixerChannel, Mathf.Log10(volume) * 20);
            }
        }
    }

    [System.Serializable]
    public class MusicClip
    {
        public string name;
        public AudioClip Clip;
    }
}
