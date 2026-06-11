using UnityEngine;
using UnityEngine.Audio;

namespace CloudeDev.Audio
{
    public class AudioManager : MonoBehaviour
    {
        [Header("AudioSource")]
        [SerializeField] private AudioSource _musicSource;
        [SerializeField] private AudioSource _sfxSource;
        [SerializeField] private AudioMixer _audioMixer;

        [Header("AudioClips")]
        [SerializeField] private AudioClip[] _sfxClips;
        [SerializeField] private AudioClip[] _musicClips;

        private bool _isEnabledSoundMusic = true;
        private bool _isEnabledSoundSFX = true;

        public void PlaySound(AudioSource audioSource, AudioClip clip)
        {
            StopAudio(audioSource);
            audioSource.clip = clip;
        }

        public void SetSoundVolume(AudioSource audio, bool isEnabledSound)
        {
            if (isEnabledSound)
            {
                _audioMixer.SetFloat("Volume", -6f);
                audio.Play();
            }
            else
            {
                _audioMixer.SetFloat("Volume", -80f);
                StopAudio(audio);
            }
        }

        private void PlayRandomPitch(AudioSource audio, AudioClip clip)
        {
            audio.pitch = Random.Range(0.8f, 1.2f);
            audio.PlayOneShot(clip);
        }

        public void StopAudio(AudioSource audio) => audio.Stop();
        private void PlayNormalPitch(AudioSource audio, AudioClip clip) => audio.PlayOneShot(clip);
    }
}
