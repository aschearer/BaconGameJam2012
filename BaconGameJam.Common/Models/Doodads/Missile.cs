using System;
using System.Collections.ObjectModel;
using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;

namespace BaconGameJam.Common.Models.Doodads
{
    public class Missile : IDoodad
    {
        private readonly World world;
        private readonly Body body;
        private int obstacleCollisionCtr;
        private Collection<IDoodad> doodads;
        private TimeSpan elapsedTime;

        public Missile(World world, Collection<IDoodad> doodads, Team team, Vector2 position, float rotation)
        {
            this.world = world;
            this.doodads = doodads;
            this.body = BodyFactory.CreateBody(world, position, this);
            this.body.BodyType = BodyType.Dynamic;
            this.body.FixedRotation = true;

            CircleShape shape = new CircleShape(3 / Constants.PixelsPerMeter, 0);
            Fixture fixture = body.CreateFixture(shape);
            fixture.Restitution = 1;
            fixture.Friction = 0;
            fixture.CollisionCategories = PhysicsConstants.MissileCategory;
            fixture.CollidesWith = PhysicsConstants.EnemyCategory | PhysicsConstants.PlayerCategory | PhysicsConstants.ObstacleCategory;
            fixture.OnCollision += Body_OnCollision;
            obstacleCollisionCtr = 0;

            Vector2 force = new Vector2((float)Math.Cos(rotation), (float)Math.Sin(rotation)) * 600;
            this.body.ApplyForce(force);
        }

        bool Body_OnCollision(Fixture fixtureA, Fixture fixtureB, FarseerPhysics.Dynamics.Contacts.Contact contact)
        {
            if (this.elapsedTime.TotalSeconds < 0.1)
            {
                return false;
            }

            switch (fixtureB.CollisionCategories)
            {
                case PhysicsConstants.ObstacleCategory:
                    obstacleCollisionCtr++;
                    if (obstacleCollisionCtr >= 2)
                    {
                        RemoveFromGame();
                    }
                    break;
                case PhysicsConstants.EnemyCategory:
                case PhysicsConstants.PlayerCategory:
                    RemoveFromGame();
                    (fixtureB.Body.UserData as Tank).RemoveFromGame();
                    break;
            }
            return true;
        }

        public Vector2 Position
        {
            get { return this.body.Position; }
        }

        public void Update(GameTime gameTime)
        {
            this.elapsedTime += gameTime.ElapsedGameTime;
        }

        public void RemoveFromGame()
        {
            this.world.RemoveBody(this.body);
            this.doodads.Remove(this);
        }
    }
}