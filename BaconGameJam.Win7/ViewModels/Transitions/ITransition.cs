using System;
using Microsoft.Xna.Framework;

namespace BaconGameJam.Win7.ViewModels.Transitions
{
    public interface ITransition
    {
        event EventHandler<EventArgs> Closed;

        void Update(GameTime gameTime);
    }
}