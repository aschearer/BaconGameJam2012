using System;
using System.Collections.ObjectModel;
using Microsoft.Xna.Framework;

namespace BaconGameJam.Common.Models.Doodads
{
    public class Waypoint : IDoodad
    {
        private readonly Vector2 position;
        private readonly Collection<Waypoint> waypoints;

        public Waypoint(Vector2 position, string color, Collection<Waypoint> waypoints)
        {
            this.Color = color;
            this.position = position;
            this.waypoints = waypoints;
            this.waypoints.Add(this);
        }

        public Vector2 Position
        {
            get { return this.position; }
        }

        public int Column
        {
            get { return (int)((this.position.X * Constants.PixelsPerMeter) / 32); }
        }

        public int Row
        {
            get { return (int)((this.position.Y * Constants.PixelsPerMeter) / 32); }
        }

        public string Color { get; private set; }

        public void RemoveFromGame()
        {
            this.waypoints.Remove(this);
        }

        public void Update(GameTime gameTime)
        {
        }

        public override string ToString()
        {
            return string.Format("{0}: {1},{2}", this.Color, this.Column, this.Row);
        }
    }
}