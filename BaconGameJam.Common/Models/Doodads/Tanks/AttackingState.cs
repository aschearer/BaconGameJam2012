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
        private TimeSpan elapsedTime;

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
            this.elapsedTime = TimeSpan.FromSeconds(0.8);
        }

        public void Update(GameTime gameTime)
        {
            this.elapsedTime += gameTime.ElapsedGameTime;
            if (this.elapsedTime.TotalSeconds > 0.75 && this.tank.CanFireMissile(this.tank.Target.Position))
            {
                this.elapsedTime = TimeSpan.Zero;
                Vector2 delta = Vector2.Subtract(this.tank.Target.Position, this.tank.Position);
                float theta = (float)Math.Atan2(delta.Y, delta.X);
                this.tank.FireAtTarget(theta);
            }
        }
    }
}