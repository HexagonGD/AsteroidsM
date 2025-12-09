using Asteroids.Logic.Content;
using UnityEngine;

namespace Asteroids.Logic.Common.Services.Sounds
{
    public class SoundsService : MonoBehaviour
    {
        [SerializeField] private AudioSource _musicSource;
        [SerializeField] private AudioSource _sfxSource;

        private SoundsConfig _soundsConfig;

        public void ProvideSounds(SoundsConfig soundsConfig)
        {
            _soundsConfig = soundsConfig;
        }

        public void PlaySFX(SFXType sfxType)
        {
            if(_soundsConfig == null)
            {
                Debug.LogError("Sounds config isn't loaded");
                return;
            }

            var clip = sfxType switch
            {
                SFXType.DestroyedShip => _soundsConfig.ShipDestroyedClip,
                SFXType.DestroyedAsteroid => _soundsConfig.AsteroidDestroyedClip,
                SFXType.DestroyedUFO => _soundsConfig.UFODestroyedClip,
                SFXType.Shot => _soundsConfig.ShotClip,
                SFXType.Lazer => _soundsConfig.LazerClip,
                _ => null
            };

            if(clip == null)
            {
                Debug.LogError($"Not found sfx clip for {sfxType}");
                return;
            }

            _sfxSource.PlayOneShot(clip);
        }

        public void PlayMusic(MusicType musicType)
        {
            if (_soundsConfig == null)
            {
                Debug.LogError("Sounds config isn't loaded");
                return;
            }

            var clip = musicType switch
            {
                MusicType.BG => _soundsConfig.BGClip,
                _ => null
            };

            if (clip == null)
            {
                Debug.LogError($"Not found music clip for {musicType}");
                return;
            }

            if (_musicSource.clip == clip && _musicSource.isPlaying)
            {
                return;
            }

            _musicSource.clip = clip;
            _musicSource.Play();
        }

        public void StopMusic()
        {
            _musicSource.Stop();
        }
    }
}