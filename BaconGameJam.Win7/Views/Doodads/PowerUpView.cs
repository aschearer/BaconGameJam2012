using System;
using BaconGameJam.Common;
using BaconGameJam.Common.Models.Doodads;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace BaconGameJam.Win7.Views.Doodads
{
    public class PowerUpView : IRetainedControl
    {
        private readonly PowerUp powerup;
        private Texture2D texture;
        private Vector2 origin;
        private PowerUpType powerUpType;

        public PowerUpView(PowerUp powerup)
        {
            this.powerup = powerup;
            this.powerUpType = this.powerup.powerUp;
        }

        public int Layer
        {
            get { return 10; }
        }

        public void LoadContent(ContentManager content)
        {
            if (this.powerUpType == PowerUpType.Speed)
                this.texture = content.Load<Texture2D>("Images/InGame/PowerUps");
            else if (this.powerUpType == PowerUpType.UnlimitedAmmo)
                this.texture = content.Load<Texture2D>("Images/InGame/PowerUpa");
            else if (this.powerUpType == PowerUpType.ExtraBounce)
                this.texture = content.Load<Texture2D>("Images/InGame/PowerUpb");
            else
                this.texture = content.Load<Texture2D>("Images/InGame/Powerup");
            this.origin = new Vector2(this.texture.Width / 2f, this.texture.Height / 2f);
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
                this.texture,
                this.powerup.Position * Constants.PixelsPerMeter,
                null,
                Color.White,
                0,
                this.origin,
                1,
                SpriteEffects.None,
                0);
        }

        public void Dispose()
        {
        }
    }
}