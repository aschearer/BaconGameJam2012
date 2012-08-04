using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;

namespace BaconGameJam.Common.Models.Levels
{
    public class Level
    {
        private const float WorldStep = 1f / 60f;

        private readonly World world;

        public Level(World world)
        {
            this.world = world;
        }

        public void Update(GameTime gameTime)
        {
            world.Step(Level.WorldStep);
        }
    }
}