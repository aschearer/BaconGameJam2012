using System;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;

namespace BaconGameJam.Common.Models.Doodads.Tanks
{
    public class AttackingState : ITankState
    {
        public event EventHandler<StateChangeEventArgs> StateChanged;

        private readonly World world;
        private readonly Body body;
        private readonly ComputerControlledTank tank;

        public AttackingState(
            World world, 
            Body body, 
            ComputerControlledTank tank)
        {
            this.world = world;
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
            if (this.tank.CanFireMissile(this.tank.Target.Position))
            {
                Vector2 delta = Vector2.Subtract(this.tank.Target.Position, this.tank.Position);
                float theta = (float)Math.Atan2(delta.Y, delta.X);
                this.tank.FireAtTarget(theta);
            }
        }
    }
}