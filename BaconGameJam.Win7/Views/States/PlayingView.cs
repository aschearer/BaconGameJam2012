using BaconGameJam.Win7.Models.Garden;
using BaconGameJam.Win7.ViewModels.States;
using BaconGameJam.Win7.Views.Garden;
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
        private readonly FlowerView flowerView;
        private bool isContentLoaded;

        public PlayingView(
            ContentManager content, 
            SpriteBatch spriteBatch, 
            PlayingViewModel viewModel,
            FlowerView flowerView)
        {
            this.content = content;
            this.flowerView = flowerView;
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
            this.spriteBatch.Begin();
            foreach (Flower flower in this.viewModel.Flowers)
            {
                this.flowerView.Draw(gameTime, this.spriteBatch, flower);
            }

            this.spriteBatch.End();
        }

        private void LoadContent()
        {
            if (this.isContentLoaded)
            {
                return;
            }

            this.isContentLoaded = true;
            this.flowerView.LoadContent(this.content);
        }
    }
}