using System;
using System.Collections.ObjectModel;
using Microsoft.Xna.Framework;

namespace BaconGameJam.Common.Models.Doodads
{
    public class Waypoint : IDoodad
    {
        private readonly Vector2 position;
        private readonly Collection<Waypoint> waypoints;

        public Waypoint(Vector2 position, Collection<Waypoint> waypoints)
        {
            this.position = position;
            this.waypoints = waypoints;
            this.waypoints.Add(this);
        }

        public Vector2 Position
        {
            get { return this.position; }
        }

        public void RemoveFromGame()
        {
            this.waypoints.Remove(this);
        }

        public void Update(GameTime gameTime)
        {
        }
    }
}