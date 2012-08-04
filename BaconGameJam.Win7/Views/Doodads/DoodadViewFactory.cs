using System;
using BaconGameJam.Common.Models.Doodads;
using BaconGameJam.Common.Models.Levels;
using BaconGameJam.Win7.Views.Input;

namespace BaconGameJam.Win7.Views.Doodads
{
    public class DoodadViewFactory
    {
        private readonly IInputManager input;
        private readonly IKeyboardInputManager keyInput;

        public DoodadViewFactory(IInputManager input, IKeyboardInputManager keyInput)
        {
            this.input = input;
            this.keyInput = keyInput;
        }

        public IRetainedControl CreateViewFor(IDoodad doodad)
        {
            if (doodad is Wall)
            {
                return new WallView((Wall)doodad);
            }
            else if (doodad is PlayerControlledTank)
            {
                return new PlayerControlledTankView((PlayerControlledTank)doodad, this.input, this.keyInput);
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