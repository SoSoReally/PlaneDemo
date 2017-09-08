using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
namespace SoundManager
{
    [DisallowMultipleComponent]
    public class SoundSystem : MonoBehaviour
    {
        private void Awake()
        {
            value = this;
            current= CreatAudioSource();
            current.outputAudioMixerGroup=Resources.Load<AudioMixer>(GamePath.Sound.OutPut).FindMatchingGroups("SoundFx")[0];
        }

        private AudioSource current;

        public AudioSource Current
        {
            get
            {
                return current;
            }
        }

        private SoundSystem()
        {
            SoundDictionary = new Dictionary<AudioGet, AudioClip>();
         
        }

        private readonly Dictionary<AudioGet, AudioClip> SoundDictionary = new Dictionary<AudioGet, AudioClip>();

        public void LoadAllAudioClip()
        {
            SoundDictionary.Add(AudioGet.Enum(EnumWeapon.Default), Resources.Load<AudioClip>(GamePath.Sound.Weapons));

            SoundDictionary.Add(AudioGet.Enum(EnumAudio.Death), Resources.Load<AudioClip>(GamePath.Sound.Death));
        }

        private static SoundSystem value;

        public static SoundSystem Get
        {
            get
            {
                return value;
            }
        }

        public AudioClip GetAudioClip(AudioGet sound)
        {
            return SoundDictionary[sound];
        }

        private  AudioSource CreatAudioSource()
        {
            GameObject audiosoucre = new GameObject("CurrentAudioSource");

            audiosoucre.transform.SetParent(transform);

            return  audiosoucre.AddComponent<AudioSource>();
        }
    }


}

public struct AudioGet
{
    private EnumAudio enum_audio;

    private EnumAudio Enum_audio
    {
        get
        {
            return enum_audio;
        }

        set
        {
            enum_audio = value;
        }
    }

    private AudioGet(EnumAudio Enum_audio) { enum_audio = Enum_audio; }

    public static AudioGet Enum(EnumWeapon Enum_weapon)
    {
        EnumAudio ptr_enum_audio = EnumAudio.WeaponDefault;
        switch (Enum_weapon)
        {
            case EnumWeapon.Default:
                ptr_enum_audio = EnumAudio.WeaponDefault;
                break;
            default:
                break;
        }
        return new AudioGet(ptr_enum_audio);
    }

    public static AudioGet Enum(EnumAudio Enum_audio)
    {
        return new AudioGet(Enum_audio);
    }

}

public enum EnumAudio
{
    Hurt,
    Attack,
    Death,
    WeaponDefault,
}

