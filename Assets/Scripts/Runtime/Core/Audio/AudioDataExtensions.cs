using UnityEngine;

namespace Core.Services.Audio
{
    public static class AudioDataExtensions
    {
        public static AudioClip GetClip(this AudioConfig config, string clipId)
        {
            var audioData = config.Audio.Find(x => x.Id == clipId);
            return audioData.Clip;
        }
    }
}