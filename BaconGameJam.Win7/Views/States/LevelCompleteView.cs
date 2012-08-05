using System;
using BaconGameJam.Win7.ViewModels.States;
using BaconGameJam.Win7.Views.Input;
using BaconGameJam.Win7.Views.Levels;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace BaconGameJam.Win7.Views.States
{
    public class LevelCompleteView : IScreenView
    {
        private readonly ContentManager content;
        private readonly LevelCompleteViewModel viewModel;
        private readonly SpriteBatch spriteBatch;
        private readonly LevelView levelView;
        private readonly IInputManager input;
        private bool isContentLoaded;

        public LevelCompleteView(
            ContentManager content, 
            SpriteBatch spriteBatch, 
            LevelCompleteViewModel viewModel, 
            LevelView levelView, 
            IInputManager input)
        {
            this.content = content;
            this.input = input;
            this.levelView = levelView;
            this.viewModel = viewModel;
            this.spriteBatch = spriteBatch;
        }

        public void NavigateTo()
        {
            this.LoadContent();
            this.input.Click += this.OnClick;
        }

        public void NavigateFrom()
        {
            this.input.Click -= this.OnClick;
        }

        public void Update(GameTime gameTime)
        {
            this.viewModel.Update(gameTime);
        }

        public void Draw(GameTime gameTime)
        {
            this.spriteBatch.Begin();
            this.levelView.Draw(gameTime, spriteBatch);
            this.spriteBatch.End();
        }

        private void LoadContent()
        {
            if (this.isContentLoaded)
            {
                return;
            }

            this.isContentLoaded = true;
            this.levelView.LoadContent(this.content);
        }

        private void OnClick(object sender, InputEventArgs e)
        {
            this.viewModel.NextLevelCommand.Execute(null);
        }
    }
}