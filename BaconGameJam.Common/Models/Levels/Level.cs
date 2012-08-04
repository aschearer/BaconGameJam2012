using System.Collections.ObjectModel;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using BaconGameJam.Common.Models.Doodads;

namespace BaconGameJam.Common.Models.Levels
{
    public class Level
    {
        private const float WorldStep = 1f / 60f;

        private readonly World world;
        private Collection<IDoodad> doodads;

        public Level(World world, Collection<IDoodad> doodads)
        {
            this.world = world;
            this.doodads = doodads;
        }

        public void Update(GameTime gameTime)
        {
            world.Step(Level.WorldStep);
            foreach (IDoodad doodad in this.doodads)
            {
                doodad.Update(gameTime);
            }
        }
    }
}