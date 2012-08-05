using System;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;

namespace BaconGameJam.Common.Models.Doodads.Tanks
{
    public class TurningState : ITankState
    {
        private const float Torque = MathHelper.PiOver4 / 16;

        public event EventHandler<StateChangeEventArgs> StateChanged;

        private readonly Body body;
        private readonly ComputerControlledTank tank;

        public TurningState(Body body, ComputerControlledTank tank)
        {
            this.body = body;
            this.tank = tank;
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
            if (Math.Abs(this.tank.TargetRotation - this.body.Rotation) > TurningState.Torque)
            {
                float sign = Math.Sign(this.tank.TargetRotation - this.body.Rotation);
                this.body.Rotation = this.body.Rotation + (sign * TurningState.Torque);

                if (Math.Abs(this.tank.TargetRotation - this.tank.Heading) > TurningState.Torque * 2)
                {
                    this.tank.Heading += (sign * TurningState.Torque * 2);
                }
                else
                {
                    this.tank.Heading = this.tank.TargetRotation;
                }
            }
            else
            {
                this.body.Rotation = this.tank.TargetRotation;
                this.tank.Heading = this.tank.TargetRotation;
                this.StateChanged(this, new StateChangeEventArgs(typeof(MovingState)));
            }
        }
    }
}