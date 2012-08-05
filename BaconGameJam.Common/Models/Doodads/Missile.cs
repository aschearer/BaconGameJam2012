using System;
using System.Collections.ObjectModel;
using BaconGameJam.Common.Models.Sounds;
using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Contacts;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;

namespace BaconGameJam.Common.Models.Doodads
{
    public class Missile : IDoodad
    {
        private readonly ISoundManager soundManager;
        private readonly World world;
        private readonly Body body;
        private readonly Collection<IDoodad> doodads;
        private int obstacleCollisionCtr;
        private TimeSpan elapsedTime;

        public Missile(ISoundManager soundManager, World world, Collection<IDoodad> doodads, Team team, Vector2 position, float rotation)
        {
            this.soundManager = soundManager;
            this.world = world;
            this.doodads = doodads;
            this.body = BodyFactory.CreateBody(world, position, this);
            this.body.BodyType = BodyType.Dynamic;
            this.body.FixedRotation = true;

            CircleShape shape = new CircleShape(5 / Constants.PixelsPerMeter, 0.1f);
            Fixture fixture = body.CreateFixture(shape);
            fixture.Restitution = 1;
            fixture.Friction = 0;
            fixture.CollisionCategories = PhysicsConstants.MissileCategory;
            fixture.CollidesWith = PhysicsConstants.EnemyCategory | PhysicsConstants.PlayerCategory |
                                   PhysicsConstants.ObstacleCategory | PhysicsConstants.MissileCategory;
            obstacleCollisionCtr = 0;

            Vector2 force = new Vector2((float)Math.Cos(rotation), (float)Math.Sin(rotation)) * 3;
            this.body.ApplyForce(force);
        }

        public Vector2 Position
        {
            get { return this.body.Position; }
        }

        public bool IsDead { get; private set; }

        public Vector2 Velocity
        {
            get { return this.body.LinearVelocity; }
        }

        public void Update(GameTime gameTime)
        {
            this.elapsedTime += gameTime.ElapsedGameTime;

            ContactEdge edge = this.body.ContactList;
            while (edge != null)
            {
                if (edge.Contact.IsTouching())
                {
                    Fixture enemy = this.body.Equals(edge.Contact.FixtureA.Body)
                                        ? edge.Contact.FixtureB
                                        : edge.Contact.FixtureA;

                    this.HandleCollision(enemy);
                    break;
                }

                edge = edge.Next;
            }
        }

        public void RemoveFromGame()
        {
            this.IsDead = true;
            this.world.RemoveBody(this.body);
            this.doodads.Remove(this);
        }

        private void HandleCollision(Fixture fixture)
        {
            if (this.elapsedTime.TotalSeconds < 0.2)
            {
                if (fixture.Body.UserData != null)
                {
                    this.RemoveFromGame();
                }

                return;
            }

            if (fixture.Body.UserData == null)
            {
                return;
            }

            switch (fixture.CollisionCategories)
            {
                case PhysicsConstants.ObstacleCategory:
                    obstacleCollisionCtr++;
                    this.soundManager.PlaySound("MissileBounce");
                    if (obstacleCollisionCtr >= 2)
                    {
                        RemoveFromGame();
                    }
                    break;
                case PhysicsConstants.EnemyCategory:
                case PhysicsConstants.PlayerCategory:
                    RemoveFromGame();
                    this.soundManager.PlaySound("MissileBounce");
                    if (fixture.CollisionCategories == PhysicsConstants.EnemyCategory)
                    {
                        this.soundManager.PlaySound("TankDestroyed");
                    }
                    else
                    {
                        this.soundManager.PlaySound("PlayerTankDestroyed");
                    }

                    (fixture.Body.UserData as Tank).RemoveFromGame();
                    break;
                case PhysicsConstants.MissileCategory:
                    this.soundManager.PlaySound("MissileBounce");
                    this.RemoveFromGame();
                    break;
            }
        }
    }
}