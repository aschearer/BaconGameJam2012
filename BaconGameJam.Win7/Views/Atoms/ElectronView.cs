using System;
using BaconGameJam.Win7.Models.Atoms;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace BaconGameJam.Win7.Views.Atoms
{
    public class ElectronView : IControl<Electron>
    {
        private Texture2D electronTexture;
        private Vector2 electronOrigin;

        public void LoadContent(ContentManager content)
        {
            this.electronTexture = content.Load<Texture2D>("Images/InGame/Electron");
            this.electronOrigin = new Vector2(this.electronTexture.Width / 2f, this.electronTexture.Height / 2f);
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, Electron dataContext)
        {
            spriteBatch.Draw(
                this.electronTexture,
                dataContext.Position,
                null,
                Color.White,
                0,
                this.electronOrigin,
                1,
                SpriteEffects.None,
                0);
        }
    }
}