using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace BaconGameJam.Win7.Views
{
    /// <summary>
    /// Responsible for drawing a widget on the screen for a given object.
    /// </summary>
    /// <remarks>
    /// A RetainedControl holds an instance of the object it will draw meaning that
    /// it must be created and destroyed in line with the object itself. While this
    /// makes it more challenging to coordinate and uses more memory it allows us
    /// to perform more complicated effects and animations which require state.
    /// </remarks>
    public interface IRetainedControl
    {
        int Layer { get; }

        void LoadContent(ContentManager content);
        void Draw(GameTime gameTime, SpriteBatch spriteBatch);
        void Dispose();
    }
}