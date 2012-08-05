using System;

namespace BaconGameJam.Win7.Views.Input
{
    public class KeyboardEventArgs : EventArgs
    {
        private readonly bool isLeft;
        private readonly bool isRight;
        private readonly bool isUp;
        private readonly bool isDown;
        private readonly bool isStart;

        public KeyboardEventArgs(bool isLeft, bool isRight, bool isUp, bool isDown, bool isStart)
        {
            this.isLeft = isLeft;
            this.isRight = isRight;
            this.isUp = isUp;
            this.isDown = isDown;
            this.isStart = isStart;
        }

        public bool IsLeft { get { return this.isLeft; } }
        public bool IsRight { get { return this.isRight; } }
        public bool IsUp { get { return this.isUp; } }
        public bool IsDown { get { return this.isDown; } }
        public bool IsStart { get { return this.isStart; } }
    }
}