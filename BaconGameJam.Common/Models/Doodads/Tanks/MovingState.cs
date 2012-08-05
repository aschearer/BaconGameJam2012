using System;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;

namespace BaconGameJam.Common.Models.Doodads.Tanks
{
    public class MovingState : ITankState
    {
        public event EventHandler<StateChangeEventArgs> StateChanged;

        private readonly World world;
        private readonly Body body;
        private TimeSpan elapsedTime;

        public MovingState(World world, Body body)
        {
            this.world = world;
            this.body = body;
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
            float theta = this.body.Rotation - MathHelper.PiOver2;
            Vector2 direction = new Vector2((float)Math.Cos(theta), (float)Math.Sin(theta));
            direction.Normalize();
            this.body.Position += direction * 0.04f;

            Vector2 target = this.body.Position + direction * 1f;

            this.elapsedTime += gameTime.ElapsedGameTime;
            if (this.elapsedTime.TotalSeconds > 0.1)
            {
                this.world.RayCast(this.OnRayCast, this.body.Position, target);
                this.elapsedTime = TimeSpan.Zero;
            }
        }

        private float OnRayCast(Fixture fixture, Vector2 point, Vector2 normal, float fraction)
        {
            if (this.StateChanged != null)
            {
                this.StateChanged(this, new StateChangeEventArgs(typeof(TurningState)));
            }

            return fixture == null ? fraction : 0;
        }
    }
}