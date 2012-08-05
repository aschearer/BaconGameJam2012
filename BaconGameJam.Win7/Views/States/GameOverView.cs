using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using BaconGameJam.Common;
using BaconGameJam.Common.Models.Doodads;
using BaconGameJam.Win7.ViewModels.States;
using BaconGameJam.Win7.Views.Doodads;
using BaconGameJam.Win7.Views.Farseer;
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
        private readonly DebugViewXNA debugView;
        private bool isContentLoaded;

        Texture2D dummyTexture;
        Rectangle dummyRectangle;
        SpriteFont Font1;

        public GameOverView(
            ContentManager content, 
            GameOverViewModel viewModel,
            SpriteBatch spriteBatch,
            LevelView levelView,
            DebugViewXNA debugView)
        {
            this.content = content;
            this.viewModel = viewModel;
            this.spriteBatch = spriteBatch;
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
            spriteBatch.Begin();
            this.levelView.Draw(gameTime, spriteBatch);
            spriteBatch.Draw(dummyTexture, dummyRectangle, Color.Black * 0.5f);
            spriteBatch.DrawString(Font1, "Game Over", new Vector2(150, 100), Color.White);
            spriteBatch.End();
        }

        private void LoadContent()
        {
            if (this.isContentLoaded)
            {
                return;
            }

            this.isContentLoaded = true;

            this.Font1 = content.Load<SpriteFont>("SpriteFont1");
            this.levelView.LoadContent(content);

            this.dummyRectangle = new Rectangle(0, 0, 800, 600);
            this.dummyTexture = content.Load<Texture2D>("Images/InGame/Pixel");
        }

    }
}