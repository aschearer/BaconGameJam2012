using System;

namespace BaconGameJam.Common.Models.Sounds
{
    public interface ISoundManager
    {
        event EventHandler<SoundEventArgs> SoundPlayed;
        event EventHandler<EventArgs> MusicStarted;
        event EventHandler<EventArgs> MusicStopped;

        void PlayMusic();
        void PauseMusic();
        void PlaySound(string soundName);
    }
}