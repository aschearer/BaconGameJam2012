using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace BaconGameJam.Win7.Views
{
    /// <summary>
    /// Uses given SpriteBatch to draw itself
    /// </summary>
    public interface IControl
    {
        void LoadContent(ContentManager content);
        void Draw(GameTime gameTime, SpriteBatch spriteBatch);
    }
}