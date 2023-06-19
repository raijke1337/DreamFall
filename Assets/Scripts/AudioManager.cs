using Game.Extras;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Audio;

namespace Game.Managers
{    
    public class AudioManager : MonoBehaviour
    {

        public static AudioManager Instance { get; private set; }


        public List<AudioData> SoundData;
        public List<AudioData> MusicData;
        private string currentMusic;

        private Dictionary<string, AudioSource> SoundsDict = new Dictionary<string, AudioSource>();
        private Dictionary<string, AudioSource> MusicsDict = new Dictionary<string, AudioSource>();

        private void Awake()
        {
            DontDestroyOnLoad(this);
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }

            foreach (var a in SoundData)
            {
                var au = gameObject.AddComponent<AudioSource>();
                au.loop = a.Loop;
                au.clip = a.Clip;
                au.volume = a.Volume;
                au.pitch = a.Pitch;
                SoundsDict[a.ID] = au;
            } 
            foreach (var a in MusicData)
            {
                var au = gameObject.AddComponent<AudioSource>();
                au.loop = a.Loop;
                au.clip = a.Clip;
                au.volume = a.Volume;
                au.pitch = a.Pitch;
                MusicsDict[a.ID] = au;
            }

        }

        public void PlaySound (string ID)
        {
            try
            {               
                SoundsDict[ID].Play();
            }
            catch (Exception e)
            {
                Debug.LogWarning($"{e.Message} missing from Sounds when playing {ID}");
            }
        }
        public void PlayMusic(string ID)
        {
            try
            {
                if (currentMusic != null)
                {
                    MusicsDict[currentMusic].Stop();
                }
                MusicsDict[ID].Play();
                currentMusic = ID;
            }
            catch (Exception e)
            {
                Debug.LogWarning($"{e.Message} missing from Musics when playing {ID}");
            }
        }
        public void StopMusic()
        {
            if (currentMusic != null)
            {
                MusicsDict[currentMusic].Stop();
            }
        }
    }

}