using System;
using BaconGameJam.Win7.ViewModels.States;
using BaconGameJam.Win7.Views.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace BaconGameJam.Win7.Views.States
{
    public class CreditsView : IScreenView
    {
        private readonly SpriteBatch spriteBatch;
        private readonly ContentManager content;
        private readonly IInputManager input;
        private CreditsViewModel viewModel;
        private bool isContentLoaded;
        private Texture2D texture;

        public CreditsView(CreditsViewModel viewModel, SpriteBatch spriteBatch, ContentManager content, IInputManager input)
        {
            this.viewModel = viewModel;
            this.content = content;
            this.input = input;
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

        private void OnClick(object sender, InputEventArgs e)
        {
            this.viewModel.GoToTitle();
        }

        public void Update(GameTime gameTime)
        {
            
        }

        public void Draw(GameTime gameTime)
        {
            this.spriteBatch.Begin();
            this.spriteBatch.Draw(
                this.texture,
                Vector2.Zero,
                Color.White);
            this.spriteBatch.End();
        }

        private void LoadContent()
        {
            if (this.isContentLoaded)
            {
                return;
            }

            this.isContentLoaded = true;
            this.texture = this.content.Load<Texture2D>("Images/Credits/Background");
        }
    }
}