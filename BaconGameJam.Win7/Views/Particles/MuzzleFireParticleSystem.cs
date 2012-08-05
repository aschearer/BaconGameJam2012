using System;
using Microsoft.Xna.Framework;

namespace BaconGameJam.Win7.Views.Particles
{
    public class MuzzleFireParticleSystem : ParticleSystem
    {
        private const float PiOver32 = MathHelper.PiOver4 / 8;

        public MuzzleFireParticleSystem(Random random)
            : base(random, 8)
        {
        }

        public float Direction { get; set; }

        protected override void InitializeConstants()
        {
            this.textureFilename = "Images/InGame/Spark";
            this.minInitialSpeed = 180;
            this.maxInitialSpeed = 240;
            this.minAcceleration = -4;
            this.maxAcceleration = 0;
            this.minLifetime = 0.25f;
            this.maxLifetime = 0.5f;
            this.minScale = 0.75f;
            this.maxScale = 1.5f;
            this.minNumParticles = 6;
            this.maxNumParticles = 8;
            this.minRotationSpeed = -MathHelper.PiOver4 / 2.0f;
            this.maxRotationSpeed = MathHelper.PiOver4 / 2.0f;
        }

        protected override Vector2 PickRandomDirection()
        {
            var theta = this.RandomBetween(-MuzzleFireParticleSystem.PiOver32, MuzzleFireParticleSystem.PiOver32);
            theta += this.Direction;

            return new Vector2((float)Math.Cos(theta), (float)Math.Sin(theta));
        }
    }
}