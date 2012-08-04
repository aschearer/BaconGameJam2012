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
        private Vector2 startPoint, endPoint, control1, control2;

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
            this.body.BodyType = BodyType.Kinematic;
            this.Team = team;
            this.Heading = rotation;

            var shape = new PolygonShape(0);
            shape.SetAsBox(15 / Constants.PixelsPerMeter, 15 / Constants.PixelsPerMeter);
            var fixture = this.body.CreateFixture(shape);
            fixture.CollisionCategories = this.CollisionCategory;
            fixture.CollidesWith = Constants.EnemyCategory | Constants.ObstacleCategory | Constants.MissileCategory;

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
            get { return Constants.EnemyCategory; }
        }

        public void Update(GameTime gameTime)
        {
            this.OnUpdate(gameTime);

            if (this.Team == Doodads.Team.Red)
            {
                Vector2 rayStart = new Vector2(Position.X, Position.Y);
                Vector2 rayEnd = rayStart + new Vector2(0, (MovingUp ? -1 : 1));

                this.world.RayCast((fixture, point, normal, fraction) =>
                {
                    if (fixture != null)
                    {
                        MovingUp = !MovingUp;
                        return 1;
                    }
                    return fraction;
                }, rayStart, rayEnd);

                this.body.SetTransform(new Vector2(this.body.Position.X, this.body.Position.Y + (this.MovingUp ? -0.01f : 0.01f)), this.Heading);
            }
            else
            {
                // up/down raycast
                Vector2 rayStart = new Vector2(Position.X, Position.Y);
                Vector2 rayEnd = rayStart + new Vector2(0, (MovingUp ? -1 : (MovingDown ? 1 : 0)));

                this.world.RayCast((fixture, point, normal, fraction) =>
                {
                    if ((fixture != null) & (fixture.CollisionCategories == Constants.ObstacleCategory))
                    {
                        if (MovingUp) MovingUp = false;
                        else if (MovingDown) MovingDown = false;
                        return 1;
                    }
                    return fraction;
                }, rayStart, rayEnd);

                // left/right raycast
                rayStart = new Vector2(Position.X, Position.Y);
                rayEnd = rayStart + new Vector2((MovingLeft ? -1 : (MovingRight ? 1 : 0)), 0);

                this.world.RayCast((fixture, point, normal, fraction) =>
                {
                    if ((fixture != null) & (fixture.CollisionCategories == Constants.ObstacleCategory))
                    {
                        if (MovingLeft) MovingLeft = false;
                        else if (MovingRight) MovingRight = false;
                        return 1;
                    }
                    return fraction;
                }, rayStart, rayEnd);

                this.body.SetTransform(new Vector2(this.body.Position.X + (MovingLeft ? -0.05f : (MovingRight ? 0.05f : 0)), this.body.Position.Y + (MovingUp ? -0.05f : (MovingDown ? 0.05f : 0))), this.Heading);
            }
        }

        //bool Body_OnCollision(Fixture fixtureA, Fixture fixtureB, FarseerPhysics.Dynamics.Contacts.Contact contact)
        //{
        //    if (fixtureB.CollisionCategories == Constants.ObstacleCategory)
        //    {
        //        if ((this.Heading >= 0) && (this.Heading <= 180))
        //            this.Heading += 180;
        //        else if ((this.Heading > 180) && (this.Heading <= 360))
        //            this.Heading -= 180;
        //    }

        //    return true;
        //}

        public void RemoveFromGame()
        {
            this.world.RemoveBody(this.body);
            this.doodads.Remove(this);
        }

        protected void Move(Vector2 target)
        {
            this.startPoint = this.body.Position;
        }

        protected bool ContainsPoint(Vector2 point)
        {
            return this.world.TestPointAll(point).Any(fixture => this.Equals(fixture.Body.UserData));
        }

        protected virtual void OnUpdate(GameTime gameTime)
        {
            //TODO: this isn't getting called from the Update() function
            this.startPoint = this.Position;
        }
    }
}