using System;
using System.Collections.ObjectModel;
using BaconGameJam.Common.Models.Levels;
using FarseerPhysics.Dynamics;

namespace BaconGameJam.Common.Models.Doodads
{
    public class DoodadFactory
    {
        private readonly World world;
        private readonly Collection<IDoodad> doodads;

        public DoodadFactory(World world, Collection<IDoodad> doodads)
        {
            this.world = world;
            this.doodads = doodads;
        }

        public IDoodad CreateDoodad(DoodadPlacement doodadPlacement)
        {
            IDoodad doodad;
            switch (doodadPlacement.DoodadType)
            {
                case DoodadType.Tank:
                    if (doodadPlacement.Team == Team.Green)
                    {
                        doodad = new PlayerControlledTank(this, this.world, this.doodads, doodadPlacement.Team, doodadPlacement.Position, doodadPlacement.Rotation);
                    }
                    else
                    {
                        doodad = new Tank(this.world, this.doodads, doodadPlacement.Team, doodadPlacement.Position, doodadPlacement.Rotation);
                    }

                    break;
                case DoodadType.Wall:
                    doodad = new Wall(this.world, doodadPlacement.Position, doodadPlacement.Rotation, doodadPlacement.Source);
                    break;
                case DoodadType.Missile:
                    doodad = new Missile(this.world, this.doodads, doodadPlacement.Team, doodadPlacement.Position, doodadPlacement.Rotation);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            this.doodads.Add(doodad);
            return doodad;
        }
    }
}