using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace BaconGameJam.Win7.Views
{
    public interface IRetainedControl
    {
        void LoadContent(ContentManager content);
        void Draw(GameTime gameTime, SpriteBatch spriteBatch);
    }
}