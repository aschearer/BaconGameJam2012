using System;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;

namespace BaconGameJam.Common.Models.Doodads.Tanks
{
    public class TurningState : ITankState
    {
        private const float Torque = MathHelper.PiOver4 / 16;

        public event EventHandler<StateChangeEventArgs> StateChanged;

        private readonly World world;
        private readonly Body body;
        private readonly Random random;
        private readonly Tank tank;
        private float targetTheta;
        private float turningDirection;

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
            this.Probe();
        }

        public void Update(GameTime gameTime)
        {
            if (Math.Abs(this.targetTheta - this.body.Rotation) > TurningState.Torque)
            {
                float sign = Math.Sign(this.targetTheta - this.body.Rotation);
                this.body.Rotation = this.body.Rotation + (sign * TurningState.Torque);

                if (Math.Abs(this.targetTheta - this.tank.Heading) > 0.01f)
                {
                    this.tank.Heading += (sign * TurningState.Torque * 2);
                }
            }
            else
            {
                this.body.Rotation = this.targetTheta;
                this.StateChanged(this, new StateChangeEventArgs(typeof(MovingState)));
            }
        }

        private void Probe()
        {
            this.turningDirection = this.random.NextDouble() > 0.5 ? 1 : -1;
            this.targetTheta = this.body.Rotation + this.turningDirection * MathHelper.PiOver2;
            this.ProbeDirection();
        }

        private void ProbeDirection()
        {
            float adjustedTheta = this.targetTheta - MathHelper.PiOver2;
            Vector2 direction = new Vector2((float)Math.Cos(adjustedTheta), (float)Math.Sin(adjustedTheta));
            direction.Normalize();
            Vector2 target = this.body.Position + direction * 1.5f;
            this.world.RayCast(this.OnProbeReturned, this.body.Position, target);
        }

        private float OnProbeReturned(Fixture fixture, Vector2 point, Vector2 normal, float fraction)
        {
            if (fixture != null)
            {
                this.targetTheta += this.turningDirection * MathHelper.PiOver2;
                this.ProbeDirection();
                return 0;
            }

            return fraction;
        }
    }
}