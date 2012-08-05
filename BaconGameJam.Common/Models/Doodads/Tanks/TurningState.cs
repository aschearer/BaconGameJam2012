using System;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;

namespace BaconGameJam.Common.Models.Doodads.Tanks
{
    public class TurningState : ITankState
    {
        public event EventHandler<StateChangeEventArgs> StateChanged;

        private readonly World world;
        private readonly Body body;
        private float targetTheta;
        private readonly Random random;
        private readonly Tank tank;

        public TurningState(World world, Body body, Random random, Tank tank)
        {
            this.world = world;
            this.body = body;
            this.random = random;
            this.tank = tank;
        }

        public bool IsMoving
        {
            get { return true; }
        }

        public void NavigateTo()
        {
            this.ProbeDirection();
        }

        public void Update(GameTime gameTime)
        {
            if (Math.Abs(this.targetTheta - this.body.Rotation) > 0.01f)
            {
                float sign = Math.Sign(this.targetTheta - this.body.Rotation);
                this.body.SetTransform(this.body.Position, this.body.Rotation + (sign * MathHelper.PiOver4 / 16));

                if (Math.Abs(this.targetTheta - this.tank.Heading) > 0.01f)
                {
                    this.tank.Heading += (sign * MathHelper.PiOver4 / 8);
                }
            }
            else
            {
                this.StateChanged(this, new StateChangeEventArgs(typeof(MovingState)));
            }
        }

        private void ProbeDirection()
        {
            this.targetTheta = this.body.Rotation;
            this.targetTheta += this.random.NextDouble() > 0.5
                                    ? MathHelper.PiOver2
                                    : -MathHelper.PiOver2;

            Vector2 direction = new Vector2((float)Math.Cos(this.targetTheta), (float)Math.Sin(this.targetTheta));
            Vector2 target = this.body.Position + direction * 3;
            //this.world.RayCast(this.OnProbeReturned, this.body.Position, target);
        }

        private float OnProbeReturned(Fixture fixture, Vector2 point, Vector2 normal, float fraction)
        {
            if (fixture != null)
            {
                this.ProbeDirection();
                return 0;
            }

            return fraction;
        }
    }
}