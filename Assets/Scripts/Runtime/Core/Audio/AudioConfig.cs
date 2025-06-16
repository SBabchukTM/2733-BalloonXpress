using System.Collections.Generic;
using UnityEngine;

namespace Core.Services.Audio
{
    [CreateAssetMenu(fileName = "AudioConfig", menuName = "Config/AudioConfig")]
    public sealed class AudioConfig : BaseSettings
    {
        public List<AudioData> Audio;
    }
}