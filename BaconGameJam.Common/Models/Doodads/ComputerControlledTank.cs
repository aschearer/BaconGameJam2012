using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using BaconGameJam.Common.Models.Doodads.Tanks;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;

namespace BaconGameJam.Common.Models.Doodads
{
    public class ComputerControlledTank : Tank
    {
        private readonly Dictionary<Type, ITankState> states;
        private ITankState currentState;

        public ComputerControlledTank(
            World world, 
            Collection<IDoodad> doodads, 
            Team team, 
            Vector2 position, 
            float rotation,
            Random random)
            : base(world, doodads, team, position, rotation)
        {
            this.states = new Dictionary<Type, ITankState>();
            this.states.Add(typeof(MovingState), new MovingState(world, this.Body));
            this.states.Add(typeof(TurningState), new TurningState(world, this.Body, random, this));
            this.currentState = this.states[typeof(MovingState)];
            this.currentState.StateChanged += this.OnStateChanged;
            this.currentState.NavigateTo();
        }

        public override bool IsMoving
        {
            get { return this.currentState.IsMoving; }
        }

        protected override void OnUpdate(GameTime gameTime)
        {
            this.currentState.Update(gameTime);
        }

        private void OnStateChanged(object sender, StateChangeEventArgs e)
        {
            this.currentState.StateChanged -= this.OnStateChanged;
            this.currentState = this.states[e.TargetState];
            this.currentState.StateChanged += this.OnStateChanged;
            this.currentState.NavigateTo();
        }
    }
}