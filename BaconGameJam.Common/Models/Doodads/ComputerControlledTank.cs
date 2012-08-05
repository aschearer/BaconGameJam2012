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

        private readonly World world;
        private ITankState currentState;
        private readonly Body sensor;
        private Tuple<float, IDoodad> closestBody;
        private TimeSpan elapsedTime;

        public ComputerControlledTank(
            World world, 
            Collection<IDoodad> doodads, 
            Team team, 
            Vector2 position, 
            float rotation,
            Random random, 
            DoodadFactory doodadFactory)
            : base(world, doodads, team, position, rotation, doodadFactory)
        {
            this.world = world;
            this.states = new Dictionary<Type, ITankState>();
            this.states.Add(typeof(MovingState), new MovingState(world, this.Body));
            this.states.Add(typeof(AttackingState), new AttackingState(world, this.Body, this));
            this.states.Add(typeof(TurningState), new TurningState(world, this.Body, random, this));
            this.currentState = this.states[typeof(MovingState)];
            this.currentState.StateChanged += this.OnStateChanged;
            this.currentState.NavigateTo();

            this.sensor = BodyFactory.CreateBody(world, this.Position, this);

            var shape = new CircleShape(4, 0);
            Fixture sensorFixture = this.sensor.CreateFixture(shape);
            sensorFixture.Friction = 1f;
            sensorFixture.IsSensor = true;
            sensorFixture.CollisionCategories = PhysicsConstants.EnemyCategory;
            sensorFixture.CollidesWith = PhysicsConstants.PlayerCategory | PhysicsConstants.ObstacleCategory;
        }

        public override bool IsMoving
        {
            get { return this.currentState.IsMoving; }
        }

        public Tank Target { get; private set; }

        protected override void OnUpdate(GameTime gameTime)
        {
            this.sensor.SetTransform(this.Position, this.Rotation);

            if (this.closestBody != null && this.closestBody.Item2 is PlayerControlledTank)
            {
                this.Target = (Tank)this.closestBody.Item2;
                this.closestBody = null;
                this.ChangeState(this.states[typeof(AttackingState)]);
                return;
            }

            this.elapsedTime += gameTime.ElapsedGameTime;
            ContactEdge edge = this.sensor.ContactList;
            while (edge != null)
            {
                if (edge.Contact.IsTouching() && 
                    (edge.Contact.FixtureA.Body.UserData is PlayerControlledTank || edge.Contact.FixtureB.UserData is PlayerControlledTank))
                {
                    Fixture enemy = edge.Contact.FixtureA.Body.UserData is PlayerControlledTank
                                        ? edge.Contact.FixtureA
                                        : edge.Contact.FixtureB;

                    if (this.elapsedTime.TotalSeconds > 0.1)
                    {
                        this.elapsedTime = TimeSpan.Zero;
                        this.closestBody = null;
                        this.TryToTargetEnemny(((Tank)enemy.Body.UserData));
                    }

                    break;
                }

                edge = edge.Next;
            }

            this.currentState.Update(gameTime);
        }

        private void TryToTargetEnemny(Tank tank)
        {
            Vector2 delta = tank.Position - this.Position;
            delta.Normalize();
            delta *= 1;

            this.world.RayCast(this.OnProbeReturned, this.Position, tank.Position + delta);
        }

        private float OnProbeReturned(Fixture fixture, Vector2 point, Vector2 normal, float fraction)
        {
            if (this.closestBody == null)
            {
                // getting started
                this.closestBody = new Tuple<float, IDoodad>(fraction, (IDoodad)fixture.Body.UserData);
                return 1;
            }
            else if (this.closestBody.Item2 is PlayerControlledTank && this.closestBody.Item1 > fraction)
            {
                // there's a body in between us and the player
                this.closestBody = null;
                return 0;
            }
            else if (this.closestBody.Item1 > fraction)
            {
                // this body is closer to us, but we're not finished yet
                this.closestBody = new Tuple<float, IDoodad>(fraction, (IDoodad)fixture.Body.UserData);
                return 1;
            }

            return fraction;
        }

        private void OnStateChanged(object sender, StateChangeEventArgs e)
        {
            this.ChangeState(this.states[e.TargetState]);
        }

        private void ChangeState(ITankState nextState)
        {
            this.currentState.StateChanged -= this.OnStateChanged;
            this.currentState = nextState;
            this.currentState.StateChanged += this.OnStateChanged;
            this.currentState.NavigateTo();
        }
    }
}