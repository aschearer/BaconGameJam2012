using System;
using BaconGameJam.Common;
using BaconGameJam.Common.Models.Doodads;
using BaconGameJam.Win7.Views.Particles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace BaconGameJam.Win7.Views.States
{
    public class BlastMarkView : IRetainedControl
    {
        private readonly BlastMark blastMark;
        private Texture2D texture;
        private Vector2 origin;
        private Rectangle source;
        private ExplosionParticleSystem particles;
        private TimeSpan particleTimer;

        public BlastMarkView(BlastMark blastMark, Random random)
        {
            this.blastMark = blastMark;
            this.particles = new ExplosionParticleSystem(random);
        }

        public void LoadContent(ContentManager content)
        {
            this.texture = content.Load<Texture2D>("Images/InGame/Blasts");
            this.origin = new Vector2(30, 30);
            this.source = new Rectangle(0, 0, 60, 60);
            this.particles.LoadContent(content);
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            this.source.X = 60 * this.blastMark.BlastId;
            spriteBatch.Draw(
                this.texture,
                this.blastMark.Position * Constants.PixelsPerMeter,
                source,
                Color.White,
                this.blastMark.Rotation,
                this.origin,
                this.blastMark.IsSmall ? 0.25f : 0.5f,
                SpriteEffects.None,
                1);

            if (!this.blastMark.IsSmall)
            {
                this.DrawSmoke(spriteBatch, gameTime);
            }
        }

        private void DrawSmoke(SpriteBatch spriteBatch, GameTime gameTime)
        {
            if (this.particleTimer < TimeSpan.FromSeconds(0.05))
            {
                this.particleTimer += gameTime.ElapsedGameTime;
                this.particles.AddParticles(this.blastMark.Position * Constants.PixelsPerMeter);
            }

            this.particles.Draw(gameTime, spriteBatch);
        }

        public void Dispose()
        {
        }
    }
}