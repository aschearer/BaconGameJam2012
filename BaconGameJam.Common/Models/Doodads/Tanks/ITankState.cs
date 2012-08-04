using System;
using Microsoft.Xna.Framework;

namespace BaconGameJam.Common.Models.Doodads.Tanks
{
    public interface ITankState
    {
        event EventHandler<StateChangeEventArgs> StateChanged;

        bool IsMoving { get; }

        void NavigateTo();
        void Update(GameTime gameTime);
    }
}