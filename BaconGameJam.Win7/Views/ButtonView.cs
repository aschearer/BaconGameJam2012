using System;
using System.Windows.Input;
using BaconGameJam.Win7.Views.Input;
using BaconGameJam.Win7.Views.Tweens;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using InputEventArgs = BaconGameJam.Win7.Views.Input.InputEventArgs;

namespace BaconGameJam.Win7.Views
{
    /// <summary>
    /// A simple button which can be clicked.
    /// </summary>
    /// <remarks>
    /// The button will animate in response to being clicked. When clicked
    /// the provided Command is executed. In order to build more complex
    /// buttons simply extend this class and override as necessary.
    /// </remarks>
    public class ButtonView
    {
        private readonly IInputManager inputManager;

        private readonly ITween scaleTween;
        private readonly string textureName;
        private readonly Vector2 position;
        private Rectangle bounds;
        private Texture2D texture;
        private Vector2 origin;

        public ButtonView(
            IInputManager inputManager, 
            string textureName, 
            Vector2 position)
        {
            this.inputManager = inputManager;
            this.textureName = textureName;
            this.position = position;
            this.scaleTween = TweenFactory.Tween(1, 0.9f, TimeSpan.FromSeconds(.1));
            this.scaleTween.IsPaused = true;
        }

        public ICommand Command { get; set; }

        protected virtual Texture2D Texture
        {
            get { return this.texture; }
        }

        protected Vector2 Position
        {
            get { return this.position; }
        }

        protected Rectangle Bounds
        {
            get { return this.bounds; }
        }

        protected virtual bool IsVisible
        {
            get { return true; }
        }

        public void Activate()
        {
            this.inputManager.MouseDown += this.OnMouseDown;
            this.inputManager.Click += this.OnClick;
        }

        public void Deactivate()
        {
            this.inputManager.MouseDown -= this.OnMouseDown;
            this.inputManager.Click -= this.OnClick;
        }

        public void LoadContent(ContentManager content)
        {
            this.texture = content.Load<Texture2D>(this.textureName);
            this.bounds = this.texture.Bounds;
            this.bounds.X = (int)this.position.X - this.bounds.Width / 2;
            this.bounds.Y = (int)this.position.Y - this.bounds.Height / 2;
            this.origin = new Vector2(this.bounds.Width / 2f, this.bounds.Height / 2f);
            this.OnLoadContent(content);
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            this.Draw(gameTime, spriteBatch, 1);
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, float opacity)
        {
            if (!this.IsVisible)
            {
                return;
            }

            this.scaleTween.Update(gameTime);
            Color color = this.Command != null && this.Command.CanExecute(null) ? Color.White : Color.DarkGray;

            Vector2 scale = new Vector2(this.scaleTween.Value, this.scaleTween.Value);
            spriteBatch.Draw(
                this.Texture,
                this.position, 
                null,
                color * opacity,
                0,
                this.origin,
                scale,
                SpriteEffects.None,
                .68f);

            this.OnDraw(gameTime, spriteBatch);
        }

        protected virtual void OnLoadContent(ContentManager content)
        {
        }

        protected virtual void OnDraw(GameTime gameTime, SpriteBatch spriteBatch)
        {
        }

        protected virtual void Execute()
        {
            this.Command.Execute(null);
        }

        private void OnMouseDown(object sender, InputEventArgs e)
        {
            if (this.IsVisible && this.Command != null && this.Command.CanExecute(null) && this.bounds.Contains(e.X, e.Y))
            {
                //this.scaleTween.Restart();
                this.scaleTween.IsPaused = false;
                this.inputManager.DragEnded += this.OnDragEnded;
            }
        }

        private void OnDragEnded(object sender, InputEventArgs e)
        {
            if (!this.IsVisible)
            {
                return;
            }

            //this.scaleTween.Reverse();
            this.scaleTween.IsPaused = false;
            this.inputManager.DragEnded -= this.OnDragEnded;
            if (this.Command != null && this.Command.CanExecute(null) && this.bounds.Contains(e.X, e.Y))
            {
                this.Execute();
            }
        }

        private void OnClick(object sender, InputEventArgs e)
        {
            if (!this.IsVisible)
            {
                return;
            }

            if (this.Command != null && this.Command.CanExecute(null) && this.bounds.Contains(e.X, e.Y))
            {
                this.Execute();
                //this.scaleTween.Reverse();
                this.scaleTween.IsPaused = false;
                this.inputManager.DragEnded -= this.OnDragEnded;
            }
        }
    }
}