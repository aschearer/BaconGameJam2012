using System;
using System.Collections.ObjectModel;
using System.Linq;
using BaconGameJam.Common.Models.Sounds;
using FarseerPhysics.Dynamics;

namespace BaconGameJam.Common.Models.Doodads
{
    public class DoodadFactory
    {
        private readonly World world;
        private readonly Collection<IDoodad> doodads;
        private readonly Random random;
        private readonly Collection<Waypoint> waypoints;
        private readonly ISoundManager soundManager;

        public DoodadFactory(World world, Collection<IDoodad> doodads, Random random, Collection<Waypoint> waypoints, ISoundManager soundManager)
        {
            this.world = world;
            this.soundManager = soundManager;
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
                        doodad = new PlayerControlledTank(
                            this.soundManager,
                            this, 
                            this.world, 
                            this.doodads,
                            doodadPlacement.Team, 
                            this.random,
                            doodadPlacement.Position, 
                            doodadPlacement.Rotation);
                    }
                    else
                    {
                        doodad = new ComputerControlledTank(
                            this.soundManager,
                            this.world, 
                            this.doodads, 
                            doodadPlacement.Team, 
                            doodadPlacement.Position, 
                            doodadPlacement.Rotation, 
                            this.random, 
                            this,
                            this.waypoints.Where(waypoint => waypoint.Color.ToLowerInvariant() == doodadPlacement.WaypointColor.ToLowerInvariant()));
                    }

                    break;
                case DoodadType.Wall:
                    doodad = new Wall(this.world, doodadPlacement.Position, doodadPlacement.Rotation, doodadPlacement.Source);
                    break;
                case DoodadType.Missile:
                    doodad = new Missile(
                        this.soundManager,
                        this.world,
                        this.doodads,
                        doodadPlacement.NumberOfBounces,
                        doodadPlacement.Team,
                        doodadPlacement.Position,
                        doodadPlacement.Rotation,
                        this);
                    break;
                case DoodadType.Pit:
                    doodad = new Pit(
                        this.world,
                        doodadPlacement.Position,
                        doodadPlacement.Rotation,
                        doodadPlacement.Source);
                    break;
                case DoodadType.Waypoint:
                    doodad = new Waypoint(doodadPlacement.Position, doodadPlacement.WaypointColor, this.waypoints);
                    break;
                case DoodadType.BlastMark:
                    doodad = new BlastMark(doodadPlacement.Position, this.random);
                    break;
                case DoodadType.TreadMark:
                    doodad = new TreadMark(doodadPlacement.Position, doodadPlacement.Rotation, this.doodads);
                    break;
                case DoodadType.PowerUp:
                    doodad = new PowerUp(
                        this.soundManager,
                        this.world,
                        this.doodads,
                        doodadPlacement.Position,
                        this);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            this.doodads.Add(doodad);
            return doodad;
        }
    }
}