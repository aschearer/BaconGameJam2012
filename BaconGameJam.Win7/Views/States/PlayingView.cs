using BaconGameJam.Common;
using BaconGameJam.Win7.ViewModels.States;
using BaconGameJam.Win7.Views.Farseer;
using BaconGameJam.Win7.Views.Levels;
using BaconGameJam.Win7.Views.Sounds;
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
        private readonly LevelView levelView;
        private readonly DebugViewXNA debugView;
        private readonly SoundManagerView soundManagerView;
        private bool isContentLoaded;

        public PlayingView(
            ContentManager content, 
            SpriteBatch spriteBatch,
            PlayingViewModel viewModel,
            LevelView levelView,
            DebugViewXNA debugView, 
            SoundManagerView soundManagerView)
        {
            this.content = content;
            this.soundManagerView = soundManagerView;
            this.spriteBatch = spriteBatch;
            this.viewModel = viewModel;
            this.levelView = levelView;

            this.debugView = debugView;
        }

        public void NavigateTo()
        {
            this.viewModel.NavigateTo();
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
            this.spriteBatch.Begin(
                SpriteSortMode.Deferred,
                null,
                SamplerState.PointClamp,
                null,
                null);
            this.levelView.Draw(gameTime, spriteBatch);

            this.spriteBatch.End();

            if (Constants.Debug)
            {
                var matrix = Matrix.CreateOrthographicOffCenter(0f,
                                            Constants.ScreenWidth / Constants.PixelsPerMeter,
                                            Constants.ScreenHeight / Constants.PixelsPerMeter,
                                            0f,
                                            0f,
                                            1f);

                this.debugView.RenderDebugData(ref matrix);
            }
        }

        private void LoadContent()
        {
            if (this.isContentLoaded)
            {
                return;
            }

            this.isContentLoaded = true;

            this.soundManagerView.LoadContent(this.content);
            this.levelView.LoadContent(this.content);
            this.debugView.LoadContent(this.spriteBatch.GraphicsDevice, this.content);
        }
    }
}