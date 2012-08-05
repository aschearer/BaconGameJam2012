using System;
using Microsoft.Xna.Framework;

namespace BaconGameJam.Win7.Views.Particles
{
    public class ExplosionParticleSystem : ParticleSystem
    {
        public ExplosionParticleSystem(Random random)
            : base(random, 16)
        {
        }

        protected override void InitializeConstants()
        {
            this.textureFilename = "Images/InGame/Cloud";
            this.minInitialSpeed = 2;
            this.maxInitialSpeed = 64;
            this.minAcceleration = -16;
            this.maxAcceleration = 0;
            this.minLifetime = 0.25f;
            this.maxLifetime = 1.5f;
            this.minScale = 0.25f;
            this.maxScale = 1.5f;
            this.minNumParticles = 4;
            this.maxNumParticles = 8;
            this.minRotationSpeed = -MathHelper.PiOver4;
            this.maxRotationSpeed = MathHelper.PiOver4;
        }
    }
}