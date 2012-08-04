using BaconGameJam.Common.Models.Doodads;
using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;

namespace BaconGameJam.Common.Models.Levels
{
    public class Wall : IDoodad
    {
        private readonly Body body;
        private readonly World world;
        private readonly Rectangle? source;

        public Wall(World world, Vector2 position, float rotation, Rectangle source)
        {
            this.world = world;
            this.body = BodyFactory.CreateBody(world, position, this);
            this.body.Rotation = rotation;

            var shape = new PolygonShape(0);
            shape.SetAsBox(20 / Constants.PixelsPerMeter, 20 / Constants.PixelsPerMeter);
            this.body.CreateFixture(shape);

            this.source = source;
        }

        public Vector2 Position
        {
            get { return this.body.Position; }
        }

        public Rectangle? Source
        {
            get { return this.source; }
        }

        public float Rotation
        {
            get { return this.body.Rotation; }
        }

        public void RemoveFromGame()
        {
            this.world.RemoveBody(this.body);
        }
    }
}