using System;
using BaconGameJam.Win7.Models.Atoms;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace BaconGameJam.Win7.Views.Atoms
{
    public class ElectronView : IControl<Electron>
    {
        private const float OrbitTheta = MathHelper.PiOver4 / 2;

        private Texture2D electronTexture;
        private Vector2 electronOrigin;
        private Texture2D orbitTexture;
        private Vector2 orbitOrigin;

        public void LoadContent(ContentManager content)
        {
            this.electronTexture = content.Load<Texture2D>("Images/InGame/Electron");
            this.electronOrigin = new Vector2(this.electronTexture.Width / 2f, this.electronTexture.Height / 2f);
            this.orbitTexture = content.Load<Texture2D>("Images/InGame/Orbit");
            this.orbitOrigin = new Vector2(this.orbitTexture.Width / 2f, this.orbitTexture.Height / 2f);
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, Electron dataContext)
        {
            this.DrawOrbit(spriteBatch, dataContext);

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

        private void DrawOrbit(SpriteBatch spriteBatch, Electron dataContext)
        {
            float theta = dataContext.Theta - ElectronView.OrbitTheta;
            for (int i = 0; i < 4; i++)
            {
                Vector2 position = new Vector2((float)Math.Cos(theta), (float)Math.Sin(theta));
                position = Vector2.Add(dataContext.Atom.Position, position * dataContext.Radius);

                spriteBatch.Draw(
                    this.orbitTexture,
                    position,
                    null,
                    Color.White * (1f / (i + 1)),
                    0,
                    this.orbitOrigin,
                    1f / (i + 1),
                    SpriteEffects.None,
                    0);

                theta -= ElectronView.OrbitTheta;
            }
        }
    }
}