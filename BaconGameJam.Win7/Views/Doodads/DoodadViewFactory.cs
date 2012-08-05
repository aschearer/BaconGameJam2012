using System;
using BaconGameJam.Common.Models.Doodads;
using BaconGameJam.Common.Models.Levels;
using BaconGameJam.Win7.Views.Input;
using BaconGameJam.Win7.Views.States;

namespace BaconGameJam.Win7.Views.Doodads
{
    public class DoodadViewFactory
    {
        private readonly IInputManager input;
        private readonly IKeyboardInputManager keyInput;
        private readonly Random random;

        public DoodadViewFactory(IInputManager input, IKeyboardInputManager keyInput, Random random)
        {
            this.input = input;
            this.random = random;
            this.keyInput = keyInput;
        }

        public IRetainedControl CreateViewFor(IDoodad doodad)
        {
            if (doodad is Wall || doodad is TileDoodad || doodad is Pit)
            {
                return new StaticDoodadView((IStaticDoodad)doodad);
            }
            else if (doodad is PlayerControlledTank)
            {
                return new PlayerControlledTankView((PlayerControlledTank)doodad, this.input, this.keyInput, this.random);
            }
            else if (doodad is Tank)
            {
                return new TankView((Tank)doodad, this.random);
            }
            else if (doodad is Missile)
            {
                return new MissileView((Missile)doodad);
            }
            else if (doodad is Waypoint)
            {
                return new EmptyView();
            }
            else if (doodad is BlastMark)
            {
                return new BlastMarkView((BlastMark)doodad);
            }

            throw new ArgumentException("No view found for doodad");
        }
    }
}