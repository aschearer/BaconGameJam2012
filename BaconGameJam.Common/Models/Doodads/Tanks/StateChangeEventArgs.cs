using System;

namespace BaconGameJam.Common.Models.Doodads.Tanks
{
    public class StateChangeEventArgs : EventArgs
    {
        public StateChangeEventArgs(Type targetState)
        {
            this.TargetState = targetState;
        }

        public Type TargetState { get; private set; }
    }
}