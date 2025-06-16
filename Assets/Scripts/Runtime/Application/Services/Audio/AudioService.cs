using System.Collections.Generic;
using Core;
using Core.Services.Audio;
using Hellmade.Sound;
using UnityEngine;
using AudioType = Core.Services.Audio.AudioType;

namespace Runtime.Services.Audio
{
    public class AudioService : IAudioService
    {
        private readonly ISettingProvider _staticSettingsService;

        public AudioService(ISettingProvider staticSettingsService)
        {
            _staticSettingsService = staticSettingsService;
        }

        public void PlayMusic(string clipId)
        {
            var audioSettings = _staticSettingsService.Get<AudioConfig>();
            var clip = audioSettings.GetClip(clipId);
            if (clip)
                EazySoundManager.PlayMusic(clip);
        }

        public void PlayMusic(AudioClip clip)
        {
            EazySoundManager.PlayMusic(clip);
        }

        public void PlaySound(string clipId)
        {
            var audioSettings = _staticSettingsService.Get<AudioConfig>();
            var clip = audioSettings.GetClip(clipId);
            if (clip)
                EazySoundManager.PlaySound(clip);
        }

        public void PlaySound(string clipId, bool loop)
        {
            var audioSettings = _staticSettingsService.Get<AudioConfig>();
            var clip = audioSettings.GetClip(clipId);
            if (clip)
                EazySoundManager.PlaySound(clip, loop);
        }

        public void PauseAll()
        {
            EazySoundManager.PauseAll();
        }

        public void ResumeAll()
        {
            EazySoundManager.ResumeAll();
        }

        public void ResumeSounds()
        {
            EazySoundManager.ResumeAllSounds();
        }

        public bool IsPlaying(string clipId)
        {
            var audioSettings = _staticSettingsService.Get<AudioConfig>();
            var clip = audioSettings.GetClip(clipId);
            return EazySoundManager.IsPlaying(clip);
        }

        public void StopMusic()
        {
            EazySoundManager.StopAllMusic();
        }

        public void StopAllSounds()
        {
            EazySoundManager.StopAllSounds();
        }

        public void StopAll()
        {
            EazySoundManager.StopAll();
        }

        public void SetVolume(AudioType audioType, float volume)
        {
            switch (audioType)
            {
                case AudioType.Music:
                    EazySoundManager.GlobalMusicVolume = volume;
                    break;
                case AudioType.Sound:
                    EazySoundManager.GlobalSoundsVolume = volume;
                    break;
                default:
                    throw new KeyNotFoundException($"{nameof(AudioService)}: {audioType} handler not found");
            }
        }
    }
}