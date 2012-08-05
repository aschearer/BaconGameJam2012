using System;
using System.Collections.Generic;
using BaconGameJam.Common.Models.Sounds;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;

namespace BaconGameJam.Win7.Views.Sounds
{
    public class SoundManagerView
    {
        private readonly ISoundManager soundManager;
        private readonly Random random;
        private readonly Dictionary<string, List<SoundEffectInstance>> sounds;
        private Song backgroundMusic;
        private bool isPlaying;

        public SoundManagerView(ISoundManager soundManager, Random random)
        {
            this.sounds = new Dictionary<string, List<SoundEffectInstance>>();
            this.soundManager = soundManager;
            this.soundManager.SoundPlayed += this.OnSoundPlayed;
            this.soundManager.MusicStarted += this.OnMusicStarted;
            this.soundManager.MusicStopped += this.OnMusicStopped;
            this.random = random;
        }

        public void LoadContent(ContentManager content)
        {
            this.sounds["FireMissile"] = new List<SoundEffectInstance>();
            this.sounds["FireMissile"].Add(content.Load<SoundEffect>("Sounds/Gun_NailRifle_Shot01").CreateInstance());
            this.sounds["FireMissile"].Add(content.Load<SoundEffect>("Sounds/Gun_NailRifle_Shot02").CreateInstance());
            this.sounds["FireMissile"].Add(content.Load<SoundEffect>("Sounds/Gun_NailRifle_Shot03").CreateInstance());
            this.sounds["FireMissile"].Add(content.Load<SoundEffect>("Sounds/Gun_NailRifle_Shot01").CreateInstance());
            this.sounds["FireMissile"].Add(content.Load<SoundEffect>("Sounds/Gun_NailRifle_Shot02").CreateInstance());
            this.sounds["FireMissile"].Add(content.Load<SoundEffect>("Sounds/Gun_NailRifle_Shot03").CreateInstance());
            this.sounds["FireMissile"].Add(content.Load<SoundEffect>("Sounds/Gun_NailRifle_Shot01").CreateInstance());
            this.sounds["FireMissile"].Add(content.Load<SoundEffect>("Sounds/Gun_NailRifle_Shot02").CreateInstance());
            this.sounds["FireMissile"].Add(content.Load<SoundEffect>("Sounds/Gun_NailRifle_Shot03").CreateInstance());

            this.sounds["MissileBounce"] = new List<SoundEffectInstance>();
            this.sounds["MissileBounce"].Add(content.Load<SoundEffect>("Sounds/Grenade_Bounce4").CreateInstance());
            this.sounds["MissileBounce"].Add(content.Load<SoundEffect>("Sounds/Grenade_Bounce5").CreateInstance());
            this.sounds["MissileBounce"].Add(content.Load<SoundEffect>("Sounds/Grenade_Bounce6").CreateInstance());
            this.sounds["MissileBounce"].Add(content.Load<SoundEffect>("Sounds/Grenade_Bounce7").CreateInstance());
            this.sounds["MissileBounce"].Add(content.Load<SoundEffect>("Sounds/Grenade_Bounce4").CreateInstance());
            this.sounds["MissileBounce"].Add(content.Load<SoundEffect>("Sounds/Grenade_Bounce5").CreateInstance());
            this.sounds["MissileBounce"].Add(content.Load<SoundEffect>("Sounds/Grenade_Bounce6").CreateInstance());
            this.sounds["MissileBounce"].Add(content.Load<SoundEffect>("Sounds/Grenade_Bounce7").CreateInstance());

            this.sounds["TankDestroyed"] = new List<SoundEffectInstance>();
            this.sounds["TankDestroyed"].Add(content.Load<SoundEffect>("Sounds/Hit_TruckSuspensionImpact1").CreateInstance());
            this.sounds["TankDestroyed"].Add(content.Load<SoundEffect>("Sounds/Hit_TruckSuspensionImpact2").CreateInstance());
            this.sounds["TankDestroyed"].Add(content.Load<SoundEffect>("Sounds/Hit_TruckSuspensionImpact3").CreateInstance());

            this.sounds["PlayerTankDestroyed"] = new List<SoundEffectInstance>();
            this.sounds["PlayerTankDestroyed"].Add(content.Load<SoundEffect>("Sounds/Robot_Death_Powerdown").CreateInstance());

            this.backgroundMusic = content.Load<Song>("Sounds/Chimera-Derivation-3");
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Volume = 0.25f;
        }

        private void OnSoundPlayed(object sender, SoundEventArgs e)
        {
            List<SoundEffectInstance> soundEffects = this.sounds[e.SoundName];
            SoundEffectInstance soundEffect = soundEffects[this.random.Next(soundEffects.Count)];
            soundEffect.Volume = 0.25f;
            soundEffect.Play();
        }

        private void OnMusicStarted(object sender, EventArgs e)
        {
            if (this.isPlaying)
            {
                MediaPlayer.Resume();
            }
            else
            {
                MediaPlayer.Play(this.backgroundMusic);
                this.isPlaying = true;
            }
        }

        private void OnMusicStopped(object sender, EventArgs e)
        {
            MediaPlayer.Pause();
        }
    }
}
