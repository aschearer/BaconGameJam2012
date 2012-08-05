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
        private readonly Random random;
        private Collection<Waypoint> waypoints;

        public DoodadFactory(World world, Collection<IDoodad> doodads, Random random, Collection<Waypoint> waypoints)
        {
            this.world = world;
            this.waypoints = waypoints;
            this.random = random;
            this.doodads = doodads;
        }

        public IDoodad CreateDoodad(DoodadPlacement doodadPlacement)
        {
            IDoodad doodad;
            switch (doodadPlacement.DoodadType)
            {
                case DoodadType.Tile:
                    doodad = new TileDoodad(doodadPlacement.Position, doodadPlacement.Rotation, doodadPlacement.Source);
                    break;
                case DoodadType.Tank:
                    if (doodadPlacement.Team == Team.Green)
                    {
                        doodad = new PlayerControlledTank(this, this.world, this.doodads, doodadPlacement.Team, doodadPlacement.Position, doodadPlacement.Rotation);
                    }
                    else
                    {
                        doodad = new ComputerControlledTank(this.world, this.doodads, doodadPlacement.Team, doodadPlacement.Position, doodadPlacement.Rotation, this.random, this, this.waypoints);
                    }

                    break;
                case DoodadType.Wall:
                    doodad = new Wall(this.world, doodadPlacement.Position, doodadPlacement.Rotation, doodadPlacement.Source);
                    break;
                case DoodadType.Missile:
                    doodad = new Missile(this.world, this.doodads, doodadPlacement.Team, doodadPlacement.Position, doodadPlacement.Rotation);
                    break;
                case DoodadType.Pit:
                    doodad = new Pit(
                        this.world,
                        doodadPlacement.Position,
                        doodadPlacement.Rotation,
                        doodadPlacement.Source);
                    break;
                case DoodadType.Waypoint:
                    doodad = new Waypoint(doodadPlacement.Position, this.waypoints);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            this.doodads.Add(doodad);
            return doodad;
        }
    }
}