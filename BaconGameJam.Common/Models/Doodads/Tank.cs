using System.Linq;
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

        public Tank(World world, Team team, Vector2 position, float rotation)
        {
            this.world = world;
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

        protected virtual Category CollisionCategory
        {
            get { return Constants.EnemyCategory; }
        }

        public void Update(GameTime gameTime)
        {
        }

        public void RemoveFromGame()
        {
            this.world.RemoveBody(this.body);
        }

        protected bool ContainsPoint(Vector2 point)
        {
            return this.world.TestPointAll(point).Any(fixture => this.Equals(fixture.Body.UserData));
        }
    }
}