using System;
using System.Collections.Generic;
using BaconGameJam.Common.Models.Sounds;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;

namespace BaconGameJam.Win7.Views.Sounds
{
    public class SoundManagerView
    {
        private readonly ISoundManager soundManager;
        private readonly Random random;
        private readonly Dictionary<string, List<SoundEffectInstance>> sounds;
        private SoundEffectInstance backgroundMusic;
        private bool isPlaying;

        public SoundManagerView(ISoundManager soundManager, Random random)
        {
            this.sounds = new Dictionary<string, List<SoundEffectInstance>>();
            this.soundManager = soundManager;
            this.random = random;
        }

        public void Activate()
        {
            this.soundManager.SoundPlayed += this.OnSoundPlayed;
            this.soundManager.MusicStarted += this.OnMusicStarted;
            this.soundManager.MusicStopped += this.OnMusicStopped;
        }

        public void Deactivate()
        {
            this.soundManager.SoundPlayed -= this.OnSoundPlayed;
            this.soundManager.MusicStarted -= this.OnMusicStarted;
            this.soundManager.MusicStopped -= this.OnMusicStopped;
        }

        public void LoadContent(ContentManager content)
        {
            // leaving these lines as an example, replace with real content
            //this.sounds["Boing"] = new List<SoundEffectInstance>();
            //this.sounds["Boing"].Add(content.Load<SoundEffect>("Sounds/InGame/Boing1").CreateInstance());
            //this.sounds["Boing"].Add(content.Load<SoundEffect>("Sounds/InGame/Boing2").CreateInstance());
            //this.sounds["Boing"].Add(content.Load<SoundEffect>("Sounds/InGame/Boing3").CreateInstance());

            //this.sounds["Fire"] = new List<SoundEffectInstance>();
            //this.sounds["Fire"].Add(content.Load<SoundEffect>("Sounds/InGame/Fire1").CreateInstance());
            //this.sounds["Fire"].Add(content.Load<SoundEffect>("Sounds/InGame/Fire2").CreateInstance());

            //this.sounds["Click"] = new List<SoundEffectInstance>();
            //this.sounds["Click"].Add(content.Load<SoundEffect>("Sounds/Common/Click").CreateInstance());

            //this.backgroundMusic = content.Load<SoundEffect>("Sounds/Common/BackgroundMusic").CreateInstance();
            //this.backgroundMusic.IsLooped = true;
        }

        private void OnSoundPlayed(object sender, SoundEventArgs e)
        {
            List<SoundEffectInstance> soundEffects = this.sounds[e.SoundName];
            soundEffects[this.random.Next(soundEffects.Count)].Play();
        }

        private void OnMusicStarted(object sender, EventArgs e)
        {
            if (this.isPlaying)
            {
               this.backgroundMusic.Resume();
            }
            else
            {
                this.backgroundMusic.Play();
                this.isPlaying = true;
            }
        }

        private void OnMusicStopped(object sender, EventArgs e)
        {
            this.backgroundMusic.Pause();
        }
    }
}
