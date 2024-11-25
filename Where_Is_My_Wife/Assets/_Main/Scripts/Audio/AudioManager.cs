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
        [Range(0, 1), SerializeField] private float _musicVolumeMultiplier = 0.25f;
        [Range(0, 1), SerializeField] private float _sfxVolumeMultiplier = 0.7f;

        public const string MusicMixerVolumeParameterName = "Music";
        public const string SfxMixerVolumeParameterName = "SFX";
        
        public static float MusicVolumeValue => DataSaveManager.Instance.GetData<float>(DataSaveManager.MusicVolumeKey);
        public static float SfxVolumeValue  => DataSaveManager.Instance.GetData<float>(DataSaveManager.SfxVolumeKey);
        
        private Sequence _fadeSequence;
        
        private void Start()
        {
            SetMixerVolume(MusicVolumeValue, MusicMixerVolumeParameterName);
            SetMixerVolume(SfxVolumeValue, SfxMixerVolumeParameterName);
            
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
            const float minVolume = 0.0001f; // Never reach to zero
            
            switch (mixerChannel)
            {
                case MusicMixerVolumeParameterName:
                    _mixer.SetFloat(mixerChannel, Mathf.Log10((volume + minVolume) * _musicVolumeMultiplier) * 20);
                    DataSaveManager.Instance.SetData(DataSaveManager.MusicVolumeKey, volume);
                    break;
                case SfxMixerVolumeParameterName:
                    _mixer.SetFloat(mixerChannel, Mathf.Log10((volume + minVolume) * _sfxVolumeMultiplier)  * 20);
                    DataSaveManager.Instance.SetData(DataSaveManager.SfxVolumeKey, volume);
                    break;
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
