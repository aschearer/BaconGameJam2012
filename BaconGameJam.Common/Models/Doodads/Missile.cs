using System;
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

        public Missile(World world, Team team, Vector2 position, float rotation)
        {
            this.world = world;
            this.body = BodyFactory.CreateBody(world, position, this);
            this.body.BodyType = BodyType.Dynamic;
            this.body.FixedRotation = true;

            CircleShape shape = new CircleShape(3 / Constants.PixelsPerMeter, 0);
            Fixture fixture = body.CreateFixture(shape);
            fixture.Restitution = 1;
            fixture.Friction = 0;
            fixture.CollisionCategories = Constants.MissileCategory;
            fixture.CollidesWith = Constants.EnemyCategory | Constants.ObstacleCategory;

            Vector2 force = new Vector2((float)Math.Cos(rotation), (float)Math.Sin(rotation)) * 600;
            this.body.ApplyForce(force);
        }

        public Vector2 Position
        {
            get { return this.body.Position; }
        }

        public void RemoveFromGame()
        {
            this.world.RemoveBody(this.body);
        }
    }
}