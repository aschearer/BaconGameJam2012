using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace BaconGameJam.Win7.Views.States
{
    public class PlayingView : IScreenView
    {
        private ContentManager content;
        private SpriteBatch spriteBatch;
        private bool isContentLoaded;

        public PlayingView(ContentManager content, SpriteBatch spriteBatch)
        {
            this.content = content;
            this.spriteBatch = spriteBatch;
        }

        public void NavigateTo()
        {
            this.LoadContent();
        }

        public void NavigateFrom()
        {
        }

        public void Update(GameTime gameTime)
        {
        }

        public void Draw(GameTime gameTime)
        {
        }

        private void LoadContent()
        {
            if (this.isContentLoaded)
            {
                return;
            }

            this.isContentLoaded = true;
        }
    }
}