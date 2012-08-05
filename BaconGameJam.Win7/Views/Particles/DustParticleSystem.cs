using System;
using Microsoft.Xna.Framework;

namespace BaconGameJam.Win7.Views.Particles
{
    public class DustParticleSystem : ParticleSystem
    {
        public DustParticleSystem(Random random)
            : base(random, 30)
        {
        }

        protected override void InitializeConstants()
        {
            this.textureFilename = "Images/InGame/Dust";
            this.minInitialSpeed = 4;
            this.maxInitialSpeed = 8;
            this.minAcceleration = 0;
            this.maxAcceleration = 0;
            this.minLifetime = 0.5f;
            this.maxLifetime = 1.5f;
            this.minScale = 0.5f;
            this.maxScale = 1.5f;
            this.minNumParticles = 2;
            this.maxNumParticles = 2;
            this.minRotationSpeed = -MathHelper.PiOver4 / 2.0f;
            this.maxRotationSpeed = MathHelper.PiOver4 / 2.0f;
        }
    }
}