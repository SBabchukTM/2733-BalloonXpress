using System;
using UnityEngine;

namespace Core.Services.Audio
{
    [Serializable]
    public class AudioData
    {
        public string Id;
        public AudioType AudioType;
        public AudioClip Clip;
    }
}