using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using BaconGameJam.Common.Models.Doodads.Tanks;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Contacts;
using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;

namespace BaconGameJam.Common.Models.Doodads
{
    public class ComputerControlledTank : Tank
    {
        private readonly Dictionary<Type, ITankState> states;
        private ITankState currentState;
        private Body SensorBody;

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
            this.states.Add(typeof(AttackingState), new AttackingState(world, this.Body, this));
            this.states.Add(typeof(TurningState), new TurningState(world, this.Body, random, this));
            this.currentState = this.states[typeof(MovingState)];
            this.currentState.StateChanged += this.OnStateChanged;
            this.currentState.NavigateTo();

            SensorBody = BodyFactory.CreateBody(world, this.Position);
            SensorBody.CollisionCategories = Constants.EnemyCategory;
            SensorBody.CollidesWith = Constants.PlayerCategory;
            SensorBody.BodyType = BodyType.Dynamic;

            var shape = new CircleShape(4, 0.0000001f);
            Fixture f = SensorBody.CreateFixture(shape);
            f.IsSensor = true;
            f.CollisionCategories = Constants.EnemyCategory;
            f.CollidesWith = Constants.PlayerCategory;
            //f.CollidesWith = Constants.PlayerCategory;
        }

        public override bool IsMoving
        {
            get { return this.currentState.IsMoving; }
        }

        protected override void OnUpdate(GameTime gameTime)
        {
            this.SensorBody.SetTransform(this.Position, this.Rotation);

            ContactEdge edge = this.SensorBody.ContactList;
            while (edge != null)
            {
                System.Diagnostics.Debug.WriteLine(DateTime.Now.ToString() + " " + edge.Contact.FixtureA.CollisionCategories);
                if (edge.Contact.IsTouching())
                {
                    Fixture f = edge.Contact.FixtureA.Body == this.SensorBody
                                              ? edge.Contact.FixtureB
                                              : edge.Contact.FixtureA;
                    System.Diagnostics.Debug.WriteLine(DateTime.Now.ToString() + " " + f.CollisionCategories);
                }

                edge = edge.Next;
            }

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