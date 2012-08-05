using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace BaconGameJam.Win7.Views
{
    public interface IRetainedControl
    {
        int Layer { get; }

        void LoadContent(ContentManager content);
        void Draw(GameTime gameTime, SpriteBatch spriteBatch);
        void Dispose();
    }
}