using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace BaconGameJam.Win7.Views.Doodads
{
    public class EmptyView : IRetainedControl
    {
        public int Layer
        {
            get { return 0; }
        }

        public void LoadContent(ContentManager content)
        {
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
        }

        public void Dispose()
        {
        }
    }
}