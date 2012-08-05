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
        private readonly Tank tank;

        public AttackingState(World world, Body body, Tank tank)
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
            
        }
    }
}