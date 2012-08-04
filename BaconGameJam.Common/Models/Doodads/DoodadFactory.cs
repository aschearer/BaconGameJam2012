using System;
using BaconGameJam.Common.Models.Levels;
using FarseerPhysics.Dynamics;

namespace BaconGameJam.Common.Models.Doodads
{
    public class DoodadFactory
    {
        private readonly World world;

        public DoodadFactory(World world)
        {
            this.world = world;
        }

        public IDoodad CreateDoodad(DoodadPlacement doodadPlacement)
        {
            switch (doodadPlacement.DoodadType)
            {
                case DoodadType.Tank:
                    if (doodadPlacement.Team == Team.Green)
                    {
                        return new PlayerControlledTank(this.world, doodadPlacement.Team, doodadPlacement.Position, doodadPlacement.Rotation);
                    }

                    return new Tank(this.world, doodadPlacement.Team, doodadPlacement.Position, doodadPlacement.Rotation);
                case DoodadType.Wall:
                    return new Wall(this.world, doodadPlacement.Position, doodadPlacement.Rotation, doodadPlacement.Source);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}