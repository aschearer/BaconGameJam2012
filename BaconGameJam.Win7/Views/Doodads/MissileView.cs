using System;
using BaconGameJam.Common;
using BaconGameJam.Common.Models.Doodads;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace BaconGameJam.Win7.Views.Doodads
{
    public class MissileView : IRetainedControl
    {
        private readonly Missile missile;
        private Texture2D texture;
        private Vector2 origin;

        public MissileView(Missile missile)
        {
            this.missile = missile;
        }

        public void LoadContent(ContentManager content)
        {
            this.texture = content.Load<Texture2D>("Images/InGame/Missile");
            this.origin = new Vector2(this.texture.Width / 2f, this.texture.Height / 2f);
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
                this.texture,
                this.missile.Position * Constants.PixelsPerMeter,
                null,
                Color.White,
                0,
                this.origin,
                1,
                SpriteEffects.None,
                0);
        }
    }
}