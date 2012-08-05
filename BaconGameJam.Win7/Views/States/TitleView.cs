using System;
using BaconGameJam.Win7.ViewModels.States;
using BaconGameJam.Win7.Views.Input;
using BaconGameJam.Win7.Views.Levels;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace BaconGameJam.Win7.Views.States
{
    public class TitleView : IScreenView
    {
        private readonly ContentManager content;
        private readonly TitleViewModel viewModel;
        private readonly SpriteBatch spriteBatch;
        private readonly LevelView levelView;
        private readonly IInputManager input;
        private bool isContentLoaded;
        private readonly ButtonView playButton;
        private readonly ButtonView creditsButton;
        private Texture2D overlayTexture;

        public TitleView(
            ContentManager content,
            TitleViewModel viewModel,
            SpriteBatch spriteBatch,
            LevelView levelView,
            IInputManager input)
        {
            this.content = content;
            this.viewModel = viewModel;
            this.spriteBatch = spriteBatch;
            this.levelView = levelView;
            this.input = input;
            this.creditsButton = new ButtonView(input, "Images/Title/CreditsButton", new Vector2(388, 273));
            this.creditsButton.Command = this.viewModel.ShowCreditsCommand;
            this.playButton = new ButtonView(input, "Images/Title/PlayButton", new Vector2(406, 344));
            this.playButton.Command = this.viewModel.StartGameCommand;
        }

        public void NavigateTo()
        {
            this.LoadContent();
            this.playButton.Activate();
            this.creditsButton.Activate();
        }

        public void NavigateFrom()
        {
            this.playButton.Deactivate();
            this.creditsButton.Deactivate();
        }

        public void Update(GameTime gameTime)
        {
        }

        public void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            this.levelView.Draw(gameTime, spriteBatch);
            spriteBatch.Draw(this.overlayTexture, Vector2.Zero, Color.White);
            this.playButton.Draw(gameTime, this.spriteBatch);
            this.creditsButton.Draw(gameTime, this.spriteBatch);
            spriteBatch.End();
        }

        private void LoadContent()
        {
            if (this.isContentLoaded)
            {
                return;
            }

            this.isContentLoaded = true;

            this.levelView.LoadContent(content);

            this.overlayTexture = content.Load<Texture2D>("Images/Title/Background");
            this.playButton.LoadContent(this.content);
            this.creditsButton.LoadContent(this.content);
        }
    }
}