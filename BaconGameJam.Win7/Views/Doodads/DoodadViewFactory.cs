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
        private readonly Level level;

        public DoodadViewFactory(IInputManager input, IKeyboardInputManager keyInput, Random random, Level level)
        {
            this.input = input;
            this.level = level;
            this.random = random;
            this.keyInput = keyInput;
        }

        public IRetainedControl CreateViewFor(IDoodad doodad)
        {
            if (doodad is Wall || doodad is TileDoodad || doodad is Pit)
            {
                return new StaticDoodadView((IStaticDoodad)doodad, level);
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
                return new WaypointView((Waypoint)doodad);
            }
            else if (doodad is BlastMark)
            {
                return new BlastMarkView((BlastMark)doodad, this.random);
            }
            else if (doodad is TreadMark)
            {
                return new TreadMarkView((TreadMark)doodad);
            }
            else if (doodad is PowerUp)
            {
                return new PowerUpView((PowerUp)doodad);
            }

            throw new ArgumentException("No view found for doodad");
        }
    }
}