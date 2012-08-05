using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace BaconGameJam.Win7.Views.Levels
{
    public class LevelView
    {
        private Texture2D texture;
        private Rectangle source;

        public void LoadContent(ContentManager content)
        {
            this.texture = content.Load<Texture2D>("TileSets/ConcreteStormTileSet");
            this.source = new Rectangle(40, 0, 40, 40);
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {

        }
    }
}