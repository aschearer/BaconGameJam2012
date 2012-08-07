using BaconGameJam.Win7.ViewModels.States;
using BaconGameJam.Win7.Views.Input;
using BaconGameJam.Win7.Views.Levels;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace BaconGameJam.Win7.Views.States
{
    /// <summary>
    /// Draws the screen when the player has beaten or been defeated a level.
    /// </summary>
    /// <remarks>
    /// Draws the level and an overlay on top of it. Updates the view model
    /// in order to drive any logic it contains. Sets up buttons to forward
    /// input through commands ot the model/viewmodel layer.
    /// </remarks>
    public class LevelCompleteView : IScreenView
    {
        private readonly ContentManager content;
        private readonly LevelCompleteViewModel viewModel;
        private readonly SpriteBatch spriteBatch;
        private readonly LevelView levelView;
        private readonly ButtonView nextLevelButton;
        private bool isContentLoaded;
        private Rectangle overlayBounds;
        private Texture2D overlay;

        public LevelCompleteView(
            ContentManager content, 
            SpriteBatch spriteBatch, 
            LevelCompleteViewModel viewModel, 
            LevelView levelView, 
            IInputManager input)
        {
            this.content = content;
            this.levelView = levelView;
            this.viewModel = viewModel;
            this.spriteBatch = spriteBatch;
            this.nextLevelButton = new ButtonView(input, "Images/LevelComplete/NextLevelButton", new Vector2(328, 342));
            this.nextLevelButton.Command = this.viewModel.NextLevelCommand;
        }

        public void NavigateTo()
        {
            this.LoadContent();
            this.nextLevelButton.Activate();
        }

        public void NavigateFrom()
        {
            this.nextLevelButton.Deactivate();
        }

        public void Update(GameTime gameTime)
        {
            this.viewModel.Update(gameTime);
        }

        public void Draw(GameTime gameTime)
        {
            this.spriteBatch.Begin();
            this.levelView.Draw(gameTime, this.spriteBatch);

            this.spriteBatch.Draw(
                this.overlay,
                this.overlayBounds,
                Color.Black * 0.5f);

            this.nextLevelButton.Draw(gameTime, this.spriteBatch);
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
            this.overlayBounds = new Rectangle(0, 0, 512, 384);
            this.overlay = content.Load<Texture2D>("Images/InGame/Pixel"); 
            this.nextLevelButton.LoadContent(this.content);
        }
    }
}