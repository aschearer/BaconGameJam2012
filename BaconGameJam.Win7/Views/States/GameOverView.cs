using System;
using BaconGameJam.Win7.ViewModels.States;
using BaconGameJam.Win7.Views.Input;
using BaconGameJam.Win7.Views.Levels;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace BaconGameJam.Win7.Views.States
{
    public class GameOverView : IScreenView
    {
        private readonly ContentManager content;
        private readonly GameOverViewModel viewModel;
        private readonly SpriteBatch spriteBatch;
        private readonly LevelView levelView;
        private readonly IInputManager input;
        private bool isContentLoaded;
        private readonly ButtonView restartLevelButton;
        private Texture2D overlayTexture;
        private Rectangle overlayBounds;

        public GameOverView(
            ContentManager content, 
            GameOverViewModel viewModel,
            SpriteBatch spriteBatch,
            LevelView levelView,
            IInputManager input)
        {
            this.content = content;
            this.viewModel = viewModel;
            this.spriteBatch = spriteBatch;
            this.levelView = levelView;
            this.input = input;
            this.restartLevelButton = new ButtonView(input, "Images/GameOver/RestartLevelButton", new Vector2(328, 342));
            this.restartLevelButton.Command = this.viewModel.NewGameCommand;
        }

        public void NavigateTo()
        {
            this.LoadContent();
            this.restartLevelButton.Activate();
        }

        public void NavigateFrom()
        {
            this.restartLevelButton.Deactivate();
        }

        public void Update(GameTime gameTime)
        {
            this.viewModel.Update(gameTime);
        }

        public void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            this.levelView.Draw(gameTime, spriteBatch);
            spriteBatch.Draw(this.overlayTexture, this.overlayBounds, Color.Black * 0.5f);
            this.restartLevelButton.Draw(gameTime, this.spriteBatch);
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

            this.overlayBounds = new Rectangle(0, 0, 512, 384);
            this.overlayTexture = content.Load<Texture2D>("Images/InGame/Pixel");
            this.restartLevelButton.LoadContent(this.content);
        }
    }
}