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
        private ITankState interruptedState;
        private readonly Body sensor;
        private Tuple<float, IDoodad> closestTarget;
        private TimeSpan elapsedTime;

        public ComputerControlledTank(
            World world, 
            Collection<IDoodad> doodads, 
            Team team, 
            Vector2 position, 
            float rotation,
            Random random, 
            DoodadFactory doodadFactory,
            IEnumerable<Waypoint> waypoints)
            : base(world, doodads, team, position, rotation, doodadFactory)
        {
            this.world = world;
            this.states = new Dictionary<Type, ITankState>();
            this.states.Add(typeof(MovingState), new MovingState(world, this.Body, this, waypoints, random));
            this.states.Add(typeof(AttackingState), new AttackingState(world, this.Body, this));
            this.states.Add(typeof(TurningState), new TurningState(this.Body, this));
            this.currentState = this.states[typeof(MovingState)];
            this.currentState.StateChanged += this.OnStateChanged;
            this.currentState.NavigateTo();

            this.sensor = BodyFactory.CreateBody(world, this.Position);

            var shape = new CircleShape(8, 0);
            Fixture sensorFixture = this.sensor.CreateFixture(shape);
            sensorFixture.Friction = 1f;
            sensorFixture.IsSensor = true;
            sensorFixture.CollisionCategories = PhysicsConstants.EnemyCategory;
            sensorFixture.CollidesWith = PhysicsConstants.PlayerCategory | PhysicsConstants.ObstacleCategory |
                                         PhysicsConstants.MissileCategory;
        }

        public override bool IsMoving
        {
            get { return this.currentState.IsMoving; }
        }

        public Tank Target { get; private set; }

        public float TargetRotation { get; set; }

        protected override void OnRemoveFromGame(World world)
        {
            world.RemoveBody(this.sensor);
        }

        protected override void OnUpdate(GameTime gameTime)
        {
            this.sensor.SetTransform(this.Position, this.Rotation);

            if (!(this.currentState is AttackingState) &&
                this.closestTarget != null && 
                this.closestTarget.Item2 is PlayerControlledTank)
            {
                this.Target = (Tank)this.closestTarget.Item2;
                this.closestTarget = null;
                this.interruptedState = this.currentState;
                this.ChangeState(this.states[typeof(AttackingState)]);
                return;
            }

            if (this.currentState is AttackingState &&
                this.closestTarget != null &&
                !(this.closestTarget.Item2 is PlayerControlledTank))
            {
                this.Target = null;
                this.closestTarget = null;
                this.ChangeState(this.interruptedState);
                this.interruptedState = null;
            }

            this.elapsedTime += gameTime.ElapsedGameTime;

            bool isEnemyInRange = false;
            ContactEdge edge = this.sensor.ContactList;
            while (edge != null)
            {
                if (edge.Contact.IsTouching() && 
                    (edge.Contact.FixtureA.Body.UserData is PlayerControlledTank || edge.Contact.FixtureB.UserData is PlayerControlledTank))
                {
                    Fixture enemy = edge.Contact.FixtureA.Body.UserData is PlayerControlledTank
                                        ? edge.Contact.FixtureA
                                        : edge.Contact.FixtureB;

                    isEnemyInRange = true;
                    if (this.elapsedTime.TotalSeconds > 0.1)
                    {
                        this.elapsedTime = TimeSpan.Zero;
                        this.closestTarget = null;
                        this.TryToTargetEnemny(((Tank)enemy.Body.UserData));
                    }

                    break;
                }

                if (edge.Contact.FixtureA.Body.UserData is Missile ||
                    edge.Contact.FixtureB.Body.UserData is Missile)
                {
                    Missile missile = edge.Contact.FixtureA.Body.UserData is Missile
                                          ? edge.Contact.FixtureA.Body.UserData as Missile
                                          : edge.Contact.FixtureB.Body.UserData as Missile;

                    this.TestForDanger(missile);
                }

                edge = edge.Next;
            }

            if (!isEnemyInRange && this.currentState is AttackingState)
            {
                this.Target = null;
                this.closestTarget = null;
                this.ChangeState(this.interruptedState);
                this.interruptedState = null;
            }

            this.currentState.Update(gameTime);
        }

        private void TestForDanger(Missile missile)
        {
            Vector2 delta = missile.Velocity;
            delta.Normalize();
            delta *= Vector2.Subtract(missile.Position, this.Position).Length();

            IDoodad doodad = null;
            float minFraction = float.MaxValue;
            this.world.RayCast((f, p, n, fr) =>
                                   {
                                       if (!(f.Body.UserData is Pit))
                                       {
                                           if (fr < minFraction)
                                           {
                                               minFraction = fr;
                                               doodad = f.Body.UserData as IDoodad;
                                           }

                                           return 1;
                                       }
                                       else
                                       {
                                           return -1;
                                       }
                                   },
                                   missile.Position, 
                                   missile.Position + delta);

            // in the calling method we established there was a missle near us.
            // after the ray cast finishes we know whether the missile is on a
            // collision course with this tank. If it is try to shoot it down!
            if (doodad != null && doodad.Equals(this))
            {
                if (this.CanFireMissile(missile.Position))
                {
                    Vector2 missileDelta = Vector2.Subtract(missile.Position, this.Position);
                    float theta = (float)Math.Atan2(missileDelta.Y, missileDelta.X);
                    this.FireAtTarget(theta);
                }
            }
        }

        private void TryToTargetEnemny(Tank tank)
        {
            Vector2 delta = tank.Position - this.Position;
            delta.Normalize();
            delta *= 1;

            this.world.RayCast(this.OnTargetingProbeReturned, this.Position, tank.Position + delta);
        }

        private float OnTargetingProbeReturned(Fixture fixture, Vector2 point, Vector2 normal, float fraction)
        {
            if (this.closestTarget == null)
            {
                // getting started
                if (fixture.Body.UserData is Pit)
                {
                    return 1;
                }

                this.closestTarget = new Tuple<float, IDoodad>(fraction, (IDoodad)fixture.Body.UserData);
                return 1;
            }
            else if (this.closestTarget.Item2 is PlayerControlledTank && this.closestTarget.Item1 > fraction)
            {
                // there's a body in between us and the player
                this.closestTarget = null;
                return 0;
            }
            else if (this.closestTarget.Item1 > fraction)
            {
                // this body is closer to us, but we're not finished yet
                this.closestTarget = new Tuple<float, IDoodad>(fraction, (IDoodad)fixture.Body.UserData);
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