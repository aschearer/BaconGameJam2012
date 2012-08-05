using System;
using System.ComponentModel;
using BaconGameJam.Common.Models.Sessions;

namespace BaconGameJam.Common.Models.Sounds
{
    public class SoundManager : ISoundManager
    {
        public event EventHandler<SoundEventArgs> SoundPlayed;
        public event EventHandler<EventArgs> MusicStarted;
        public event EventHandler<EventArgs> MusicStopped;

        private readonly GameSettings settings;
        private bool isMusicPlaying;

        public SoundManager(GameSettings settings)
        {
            this.settings = settings;
            this.settings.PropertyChanged += this.OnSettingsChanged;
        }

        public void PlayMusic()
        {
            if (!this.settings.IsMusicEnabled)
            {
                return;
            }

            if (this.MusicStarted != null)
            {
                this.MusicStarted(this, new EventArgs());
                this.isMusicPlaying = true;
            }
        }

        public void PauseMusic()
        {
            if (this.MusicStarted != null)
            {
                this.MusicStopped(this, new EventArgs());
                this.isMusicPlaying = false;
            }
        }

        public void PlaySound(string soundName)
        {
            if (!this.settings.IsSoundEnabled)
            {
                return;
            }

            if (this.SoundPlayed != null)
            {
                this.SoundPlayed(this, new SoundEventArgs(soundName));
            }
        }

        private void OnSettingsChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "IsMusicEnabled")
            {
                if (this.settings.IsMusicEnabled && !this.isMusicPlaying)
                {
                    this.PlayMusic();
                }

                if (!this.settings.IsMusicEnabled && this.isMusicPlaying)
                {
                    this.PauseMusic();
                }
            }
        }
    }
}
