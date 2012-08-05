using System;
using System.Collections.Generic;
using System.Linq;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;

namespace BaconGameJam.Common.Models.Doodads.Tanks
{
    public class MovingState : ITankState
    {
        public event EventHandler<StateChangeEventArgs> StateChanged;

        private readonly World world;
        private readonly Body body;
        private readonly ComputerControlledTank tank;
        private readonly IEnumerable<Waypoint> waypoints;
        private TimeSpan elapsedTime;
        private Waypoint targetWaypoint;
        private Waypoint currentWaypoint;
        private Waypoint previousWaypoint;
        private readonly Random random;

        public MovingState(
            World world, 
            Body body, 
            ComputerControlledTank tank,
            IEnumerable<Waypoint> waypoints, 
            Random random)
        {
            this.world = world;
            this.body = body;
            this.tank = tank;
            this.random = random;
            this.waypoints = waypoints;
            this.currentWaypoint = this.GetClosestWaypoint();
            this.body.Position = this.currentWaypoint.Position;
            this.targetWaypoint = this.GetNearestNeighbor();

            Vector2 delta = Vector2.Subtract(this.targetWaypoint.Position, this.body.Position);
            float theta = (float)Math.Atan2(delta.Y, delta.X);
            this.tank.Heading = this.body.Rotation = theta + MathHelper.PiOver2;
        }

        public bool IsMoving
        {
            get { return true; }
        }

        public void NavigateTo()
        {
        }

        public void Update(GameTime gameTime)
        {
            float speed = 0.04f;
            Vector2 distanceToTarget = this.targetWaypoint.Position - this.body.Position;
            if (distanceToTarget.Length() > speed)
            {
                float theta = this.body.Rotation - MathHelper.PiOver2;
                Vector2 direction = new Vector2((float)Math.Cos(theta), (float)Math.Sin(theta));
                direction.Normalize();
                this.body.Position += direction * speed;
                this.body.Awake = true;
            }
            else
            {
                this.previousWaypoint = this.currentWaypoint;
                this.currentWaypoint = this.targetWaypoint;
                this.targetWaypoint = this.GetNearestNeighbor();

                Vector2 delta = Vector2.Subtract(this.targetWaypoint.Position, this.body.Position);
                float theta = (float)Math.Atan2(delta.Y, delta.X);
                this.tank.TargetRotation = (theta + MathHelper.PiOver2 + MathHelper.TwoPi) % MathHelper.TwoPi;

                this.StateChanged(this, new StateChangeEventArgs(typeof(TurningState)));
            }

            this.tank.TrackMove();
        }

        private Waypoint GetNearestNeighbor()
        {
            var neighbors = this.GetNeighboringWaypoints();
            var neighbor = neighbors.OrderBy(this.DistanceToWaypoint).FirstOrDefault();
            return neighbor;
        }

        private float DistanceToWaypoint(Waypoint neighbor)
        {
            return Vector2.Subtract(neighbor.Position, this.currentWaypoint.Position).Length();
        }

        private IEnumerable<Waypoint> GetNeighboringWaypoints()
        {
            List<Waypoint> neighbors = new List<Waypoint>();
            neighbors.AddRange(this.GetNeighboringWaypoints(waypoint => waypoint.Row == this.currentWaypoint.Row, waypoint => waypoint.Column));
            neighbors.AddRange(this.GetNeighboringWaypoints(waypoint => waypoint.Column == this.currentWaypoint.Column, waypoint => waypoint.Row));
            if (neighbors.Count == 0)
            {
                neighbors.Add(this.previousWaypoint);
            }

            return neighbors;
        }

        private IEnumerable<Waypoint> GetNeighboringWaypoints(Func<Waypoint, bool> select, Func<Waypoint, int> order)
        {
            List<Waypoint> neighbors = new List<Waypoint>();
            var row = this.waypoints.Where(select).OrderBy(order).ToList();
            int index = row.IndexOf(this.currentWaypoint);
            if (index > 0 && !row[index - 1].Equals(this.previousWaypoint))
            {
                neighbors.Add(row[index - 1]);
            }

            if (index < row.Count - 1 && !row[index + 1].Equals(this.previousWaypoint))
            {
                neighbors.Add(row[index + 1]);
            }

            return neighbors;
        }

        private Waypoint GetClosestWaypoint()
        {
            Tuple<float, Waypoint> nearestWaypoint = new Tuple<float, Waypoint>(
                Vector2.Subtract(this.waypoints.First().Position, this.body.Position).LengthSquared(),
                this.waypoints.First());

            var remainingWayPoints = this.waypoints.Skip(1);
            foreach (Waypoint waypoint in remainingWayPoints)
            {
                float distance = Vector2.Subtract(waypoint.Position, this.body.Position).LengthSquared();
                if (distance < nearestWaypoint.Item1)
                {
                    nearestWaypoint = new Tuple<float, Waypoint>(distance, waypoint);
                }
            }

            return nearestWaypoint.Item2;
        }
    }
}