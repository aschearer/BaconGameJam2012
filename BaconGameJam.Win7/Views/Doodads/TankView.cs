using System;
using BaconGameJam.Common;
using BaconGameJam.Common.Models.Doodads;
using BaconGameJam.Win7.Views.Particles;
using BaconGameJam.Win7.Views.Tweens;
using BaconGameJam.Win7.Views.Tweens.Easings;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace BaconGameJam.Win7.Views.Doodads
{
    public class TankView : IRetainedControl
    {
        private readonly Tank tank;
        private readonly TurretView turretView;
        private readonly ITween movementTween;
        private Texture2D texture;
        private Vector2 origin;
        private Rectangle source;
        private DustParticleSystem dustParticles;
        private MuzzleFireParticleSystem smokeParticles;

        public TankView(Tank tank, Random random)
        {
            this.tank = tank;
            this.movementTween = TweenFactory.Tween(0, 1, TimeSpan.FromSeconds(0.25), EasingFunction.Discrete);
            this.movementTween.Repeats = Repeat.Forever;
            this.movementTween.YoYos = true;
            this.turretView = new TurretView(tank);
            this.dustParticles = new DustParticleSystem(random);
            this.smokeParticles = new MuzzleFireParticleSystem(random);
        }

        public void LoadContent(ContentManager content)
        {
            string textureName = string.Format("Images/InGame/{0}Tank", this.tank.Team);
            this.texture = content.Load<Texture2D>(textureName);
            this.origin = new Vector2(25, 25);
            this.source = new Rectangle(0, 0, 50, 50);
            this.turretView.LoadContent(content);
            this.dustParticles.LoadContent(content);
            this.smokeParticles.LoadContent(content);
            this.OnLoad(content);
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (this.tank.IsFiring)
            {
                this.smokeParticles.Direction = this.tank.Heading - MathHelper.PiOver2;
                this.smokeParticles.AddParticles(this.tank.Position * Constants.PixelsPerMeter);
            }

            if (this.tank.IsMoving)
            {
                this.dustParticles.AddParticles(this.tank.Position * Constants.PixelsPerMeter);
            }

            this.dustParticles.Draw(gameTime, spriteBatch);
            this.smokeParticles.Draw(gameTime, spriteBatch);

            this.OnDraw(gameTime, spriteBatch);

            this.movementTween.IsPaused = !this.tank.IsMoving;
            this.movementTween.Update(gameTime);
            this.source.X = (int)(50 * this.movementTween.Value);

            spriteBatch.Draw(
                this.texture,
                this.tank.Position * Constants.PixelsPerMeter,
                this.source,
                Color.White,
                this.tank.Rotation,
                this.origin,
                1,
                SpriteEffects.None,
                0);

            this.turretView.Draw(gameTime, spriteBatch);
        }

        public virtual void Dispose()
        {
        }

        protected virtual void OnLoad(ContentManager content)
        {
        }

        protected virtual void OnDraw(GameTime gameTime, SpriteBatch spriteBatch)
        {
        }
    }
}