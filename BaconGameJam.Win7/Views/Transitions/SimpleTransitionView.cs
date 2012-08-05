using System;
using BaconGameJam.Win7.ViewModels.Transitions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace BaconGameJam.Win7.Views.Transitions
{
    public class SimpleTransitionView
    {
        private readonly SimpleTransitionViewModel transition;

        public SimpleTransitionView(SimpleTransitionViewModel transition)
        {
            this.transition = transition;
        }

        public void LoadContent(ContentManager content)
        {
            throw new NotImplementedException();
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {

        }

        public void Dispose()
        {
        }
    }
}