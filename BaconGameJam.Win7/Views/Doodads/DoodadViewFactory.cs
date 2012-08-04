using System;
using BaconGameJam.Common.Models.Doodads;
using BaconGameJam.Common.Models.Levels;
using BaconGameJam.Win7.Views.Input;

namespace BaconGameJam.Win7.Views.Doodads
{
    public class DoodadViewFactory
    {
        private readonly IInputManager input;

        public DoodadViewFactory(IInputManager input)
        {
            this.input = input;
        }

        public IRetainedControl CreateViewFor(IDoodad doodad)
        {
            if (doodad is Wall)
            {
                return new WallView((Wall)doodad);
            }
            else if (doodad is PlayerControlledTank)
            {
                return new PlayerControlledTankView((PlayerControlledTank)doodad, this.input);
            }
            else if (doodad is Tank)
            {
                return new TankView((Tank)doodad);
            }
            else if (doodad is Missile)
            {
                return new MissileView((Missile)doodad);
            }

            throw new ArgumentException("No view found for doodad");
        }
    }
}