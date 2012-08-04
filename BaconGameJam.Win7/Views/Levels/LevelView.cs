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
            this.texture = content.Load<Texture2D>("TileSets/CompoundTileSet");
            this.source = new Rectangle(40, 0, 40, 40);
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            for (int column = 0; column < 20; column++)
            {
                for (int row = 0; row < 12; row++)
                {
                    spriteBatch.Draw(
                        this.texture,
                        new Vector2(column * 40, row * 40), 
                        this.source,
                        Color.White);
                }
            }
        }
    }
}