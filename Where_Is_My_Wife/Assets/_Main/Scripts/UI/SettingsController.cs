using UnityEngine;
using WhereIsMyWife.Managers;

namespace WhereIsMyWife.Setting
{
    public class SettingsController : MonoBehaviour 
    { 
        [SerializeField] private SliderSetting _musicVolumeSlider;
        [SerializeField] private SliderSetting _sfxVolumeSlider;
        [SerializeField] private ToggleSetting _fullscreenToggle;

        private void Awake()
        {
            _musicVolumeSlider.OnValueChanged += SetMusicVolume;
            _sfxVolumeSlider.OnValueChanged += SetSfxVolume;
            _fullscreenToggle.OnValueChanged += SetFullscreen;
        }

        private void OnDestroy()
        {
            _musicVolumeSlider.OnValueChanged -= SetMusicVolume;
            _sfxVolumeSlider.OnValueChanged -= SetSfxVolume;
            _fullscreenToggle.OnValueChanged -= SetFullscreen;
        }

        private void Start()
        {
            _musicVolumeSlider.SetSliderNormalizedValue(AudioManager.MusicVolumeValue);
            _sfxVolumeSlider.SetSliderNormalizedValue(AudioManager.SfxVolumeValue);
            _fullscreenToggle.SetToggleState(GameManager.IsFullscreen);
        }
                
        private void SetMusicVolume(float value)
        {
            AudioManager.Instance.SetMixerVolume(value, AudioManager.MusicMixerVolumeParameterName);
        }
                
        private void SetSfxVolume(float value)
        {
            AudioManager.Instance.SetMixerVolume(value, AudioManager.SfxMixerVolumeParameterName);
        }
                
        private void SetFullscreen(bool value)
        {
            GameManager.Instance.SetFullscreen(value); 
        }
    }
}