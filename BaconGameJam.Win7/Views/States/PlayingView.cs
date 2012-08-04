using BaconGameJam.Win7.Models.Atoms;
using BaconGameJam.Win7.ViewModels.States;
using BaconGameJam.Win7.Views.Atoms;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace BaconGameJam.Win7.Views.States
{
    public class PlayingView : IScreenView
    {
        private readonly ContentManager content;
        private readonly SpriteBatch spriteBatch;
        private readonly PlayingViewModel viewModel;
        private readonly AtomView atomView;
        private bool isContentLoaded;

        public PlayingView(
            ContentManager content, 
            SpriteBatch spriteBatch, 
            PlayingViewModel viewModel,
            AtomView atomView)
        {
            this.content = content;
            this.atomView = atomView;
            this.spriteBatch = spriteBatch;
            this.viewModel = viewModel;
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
            this.viewModel.Update(gameTime);
        }

        public void Draw(GameTime gameTime)
        {
            foreach (Atom atom in this.viewModel.Atoms)
            {
                this.atomView.Draw(gameTime, this.spriteBatch, atom);
            }
        }

        private void LoadContent()
        {
            if (this.isContentLoaded)
            {
                return;
            }

            this.isContentLoaded = true;
            this.atomView.LoadContent(this.content);
        }
    }
}