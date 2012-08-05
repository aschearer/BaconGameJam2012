using System;
using BaconGameJam.Common;
using BaconGameJam.Common.Models.Doodads;
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

        public TankView(Tank tank)
        {
            this.tank = tank;
            this.movementTween = TweenFactory.Tween(0, 1, TimeSpan.FromSeconds(0.25), EasingFunction.Discrete);
            this.movementTween.Repeats = Repeat.Forever;
            this.movementTween.YoYos = true;
            this.turretView = new TurretView(tank);
        }

        public void LoadContent(ContentManager content)
        {
            this.texture = content.Load<Texture2D>("Images/InGame/Tanks");
            this.origin = new Vector2(16, 16);
            this.source = new Rectangle(0, 0, 32, 32);
            this.turretView.LoadContent(content);
            this.OnLoad(content);
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            this.OnDraw(gameTime, spriteBatch);

            this.movementTween.IsPaused = !this.tank.IsMoving;
            this.movementTween.Update(gameTime);
            this.source.X = (int)(32 * this.movementTween.Value);
            this.source.Y = ((int)this.tank.Team + 1) * 32;

            spriteBatch.Draw(
                this.texture,
                this.tank.Position * Constants.PixelsPerMeter,
                this.source,
                Color.White,
                this.tank.Rotation,
                this.origin,
                2,
                SpriteEffects.None,
                0);

            this.turretView.Draw(gameTime, spriteBatch);
        }

        protected virtual void OnLoad(ContentManager content)
        {
        }

        protected virtual void OnDraw(GameTime gameTime, SpriteBatch spriteBatch)
        {
        }
    }
}