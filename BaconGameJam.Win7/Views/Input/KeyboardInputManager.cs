using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace BaconGameJam.Win7.Views.Input
{
    public class KeyboardInputManager : IKeyboardInputManager
    {
        public event EventHandler<KeyboardEventArgs> KeyDown;

        public void Update(KeyboardState buttonState)
        {
            bool isLeft, isRight, isUp, isDown = false;

            isLeft = buttonState.IsKeyDown(Keys.Left) || buttonState.IsKeyDown(Keys.A);
            isRight = buttonState.IsKeyDown(Keys.Right) || buttonState.IsKeyDown(Keys.D);
            isUp = buttonState.IsKeyDown(Keys.Up) || buttonState.IsKeyDown(Keys.W);
            isDown = buttonState.IsKeyDown(Keys.Down) || buttonState.IsKeyDown(Keys.S);

            this.HandleKeyDown(isLeft, isRight, isUp, isDown);
        }

        private void HandleKeyDown(bool isLeft, bool isRight, bool isUp, bool isDown)
        {
            if (this.KeyDown != null)
            {
                this.KeyDown(this, new KeyboardEventArgs(isLeft, isRight, isUp, isDown));
            }
        }
    }
}