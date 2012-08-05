using System;
using System.Linq;
using System.Collections.ObjectModel;
using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;

namespace BaconGameJam.Common.Models.Doodads
{
    public abstract class Tank : IDoodad
    {
        private readonly World world;
        private readonly Body body;
        private Collection<IDoodad> doodads;

        protected Tank(
            World world, 
            Collection<IDoodad> doodads, 
            Team team, 
            Vector2 position, 
            float rotation)
        {
            this.world = world;
            this.doodads = doodads;
            this.body = BodyFactory.CreateBody(world, position, this);
            this.body.Rotation = rotation;
            this.body.BodyType = BodyType.Dynamic;
            this.Team = team;
            this.Heading = rotation;

            var shape = new PolygonShape(0);
            shape.SetAsBox(12 / Constants.PixelsPerMeter, 12 / Constants.PixelsPerMeter);
            var fixture = this.body.CreateFixture(shape);
            fixture.CollisionCategories = this.CollisionCategory;
            fixture.CollidesWith = PhysicsConstants.EnemyCategory | PhysicsConstants.ObstacleCategory | PhysicsConstants.MissileCategory;
        }

        public abstract bool IsMoving { get; }

        public Vector2 Position
        {
            get { return this.body.Position; }
        }

        public float Rotation
        {
            get { return this.body.Rotation; }
        }

        public Team Team { get; set; }

        public float Heading { get; set; }

        public bool MovingUp { get; set; }
        public bool MovingDown { get; set; }
        public bool MovingLeft { get; set; }
        public bool MovingRight { get; set; }

        protected Body Body
        {
            get { return this.body; }
        }

        protected virtual Category CollisionCategory
        {
            get { return PhysicsConstants.EnemyCategory; }
        }

        public void Update(GameTime gameTime)
        {
            this.OnUpdate(gameTime);
        }

        public void RemoveFromGame()
        {
            this.world.RemoveBody(this.body);
            this.doodads.Remove(this);
        }

        protected bool ContainsPoint(Vector2 point)
        {
            return this.world.TestPointAll(point).Any(fixture => this.Equals(fixture.Body.UserData));
        }

        protected virtual void OnUpdate(GameTime gameTime)
        {
            //this.startPoint = this.Position;
        }
    }
}