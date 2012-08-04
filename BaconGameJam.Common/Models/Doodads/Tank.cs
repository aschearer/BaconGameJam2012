using System.Linq;
using System.Collections.ObjectModel;
using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;

namespace BaconGameJam.Common.Models.Doodads
{
    public class Tank : IDoodad
    {
        private readonly World world;
        private readonly Body body;
        private Collection<IDoodad> doodads;

        public Tank(World world, Collection<IDoodad> doodads, Team team, Vector2 position, float rotation)
        {
            this.world = world;
            this.doodads = doodads;
            this.body = BodyFactory.CreateBody(world, position, this);
            this.body.Rotation = rotation;
            this.body.BodyType = BodyType.Kinematic;
            this.Team = team;
            this.Heading = rotation;
            this.MovingUp = true;

            var shape = new PolygonShape(0);
            shape.SetAsBox(15 / Constants.PixelsPerMeter, 15 / Constants.PixelsPerMeter);
            var fixture = this.body.CreateFixture(shape);
            fixture.CollisionCategories = this.CollisionCategory;
            fixture.CollidesWith = Constants.EnemyCategory | Constants.ObstacleCategory | Constants.MissileCategory;

        }

        public bool IsMoving { get; private set; }

        public Vector2 Position
        {
            get { return this.body.Position; }
        }

        public float Rotation
        {
            get { return this.body.Rotation; }
        }

        public Team Team { get; set; }

        public float Heading { get; protected set; }

        public bool MovingUp { get; protected set; }
        public bool MovingLeft { get; protected set; }

        protected virtual Category CollisionCategory
        {
            get { return Constants.EnemyCategory; }
        }

        bool Body_OnCollision(Fixture fixtureA, Fixture fixtureB, FarseerPhysics.Dynamics.Contacts.Contact contact)
        {
            if (fixtureB.CollisionCategories == Constants.ObstacleCategory)
            {
                if ((this.Heading >= 0) && (this.Heading <= 180))
                    this.Heading += 180;
                else if ((this.Heading > 180) && (this.Heading <= 360))
                    this.Heading -= 180;
            }

            return true;
        }

        public void Update(GameTime gameTime)
        {
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
    }
}