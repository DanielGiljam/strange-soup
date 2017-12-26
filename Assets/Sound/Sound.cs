// This Sound class (and the AudioManager) was made by Brackeys (https://www.youtube.com/channel/UCYbK_tjZ2OrIZFBvU6CCMiA)

using System;
using UnityEngine;
using UnityEngine.Audio;

namespace Assets.Sound
{
    [Serializable]
    public class Sound {

        public string name;

        public AudioClip clip;

        [Range(0f, 1f)]
        public float volume = .75f;
        [Range(0f, 1f)]
        public float volumeVariance = .1f;

        [Range(.1f, 3f)]
        public float pitch = 1f;
        [Range(0f, 1f)]
        public float pitchVariance = .1f;

        public bool loop = false;

        public AudioMixerGroup mixerGroup;

        [HideInInspector]
        public AudioSource source;

    }
}
