using System;

namespace BaconGameJam.Win7.Views.Input
{
    public class InputEventArgs : EventArgs
    {
        private readonly int x;
        private readonly int y;

        public InputEventArgs(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public int X
        {
            get { return this.x; }
        }

        public int Y
        {
            get { return this.y; }
        }
    }
}