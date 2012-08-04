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
            fixture.CollisionCategories = Constants.MissileCategory;
            fixture.CollidesWith = Constants.EnemyCategory | Constants.ObstacleCategory;
            fixture.OnCollision += Body_OnCollision;
            obstacleCollisionCtr = 0;

            Vector2 force = new Vector2((float)Math.Cos(rotation), (float)Math.Sin(rotation)) * 600;
            this.body.ApplyForce(force);
        }

        bool Body_OnCollision(Fixture fixtureA, Fixture fixtureB, FarseerPhysics.Dynamics.Contacts.Contact contact)
        {
            switch (fixtureB.CollisionCategories)
            {
                case Constants.ObstacleCategory:
                    obstacleCollisionCtr++;
                    if (obstacleCollisionCtr >= 2)
                    {
                        RemoveFromGame();
                    }
                    break;
                case Constants.EnemyCategory:
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

        public void RemoveFromGame()
        {
            this.world.RemoveBody(this.body);
            this.doodads.Remove(this);
        }
    }
}