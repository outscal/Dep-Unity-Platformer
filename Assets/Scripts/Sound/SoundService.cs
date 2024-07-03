using System;
using Platformer.Enemy;
using Platformer.Events;
using Platformer.Main;
using UnityEngine;

namespace Platformer.Sound
{
    public class SoundService
    {
        #region Services References
        private EventService EventService => GameService.Instance.EventService;
        #endregion

        private SoundScriptableObject soundScriptableObject;
        private AudioSource audioEffects;
        private AudioSource backgroundMusic;

        public SoundService(SoundScriptableObject soundScriptableObject, AudioSource audioEffectSource, AudioSource bgMusicSource)
        {
            this.soundScriptableObject = soundScriptableObject;
            audioEffects = audioEffectSource;
            backgroundMusic = bgMusicSource;
            PlaybackgroundMusic(SoundType.BACKGROUND_MUSIC, true);
            SubscribeToEvents();
        }

        ~SoundService()
        {
            UnsubscribeToEvents();
        }

        #region Private Functions

        private void SubscribeToEvents() 
        { 
            EventService.OnEnemyDied.AddListener(OnEnemyDied);
            EventService.OnAllEnemiesDied.AddListener(OnAllEnemiesDied);
        } 

        private void UnsubscribeToEvents() => EventService.OnEnemyDied.RemoveListener(OnEnemyDied);

        private void OnAllEnemiesDied() => PlaySoundEffects(SoundType.GAME_WON);

        private void OnEnemyDied(EnemyController deadEnemy) => PlaySoundEffects(SoundType.ENEMY_DEATH);
        
        private void PlaybackgroundMusic(SoundType soundType, bool loopSound = false)
        {
            AudioClip clip = GetSoundClip(soundType);
            if (clip != null)
            {
                backgroundMusic.loop = loopSound;
                backgroundMusic.clip = clip;
                backgroundMusic.Play();
            }
            else
                Debug.LogError("No Audio Clip selected.");
        }

        private AudioClip GetSoundClip(SoundType soundType)
        {
            Sounds sound = Array.Find(soundScriptableObject.audioList, item => item.soundType == soundType);
            if (sound.audio != null)
                return sound.audio;
            return null;
        }
        #endregion
    
        #region Public Functions
        public void PlaySoundEffects(SoundType soundType, bool loopSound = false)
        {
            AudioClip clip = GetSoundClip(soundType);
            if (clip != null)
            {
                audioEffects.loop = loopSound;
                audioEffects.clip = clip;
                audioEffects.PlayOneShot(clip);
            }
            else
                Debug.LogError($"No Audio Clip selected for sound type: {soundType}");
        }
        #endregion
    }
}