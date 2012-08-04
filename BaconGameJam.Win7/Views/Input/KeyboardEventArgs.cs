using System;

namespace BaconGameJam.Win7.Views.Input
{
    public class KeyboardEventArgs : EventArgs
    {
        private readonly bool isLeft;
        private readonly bool isRight;
        private readonly bool isUp;
        private readonly bool isDown;

        public KeyboardEventArgs(bool isLeft, bool isRight, bool isUp, bool isDown)
        {
            this.isLeft = isLeft;
            this.isRight = isRight;
            this.isUp = isUp;
            this.isDown = isDown;
        }

        public bool IsLeft { get { return this.isLeft; } }
        public bool IsRight { get { return this.isRight; } }
        public bool IsUp { get { return this.isUp; } }
        public bool IsDown { get { return this.isDown; } }
    }
}