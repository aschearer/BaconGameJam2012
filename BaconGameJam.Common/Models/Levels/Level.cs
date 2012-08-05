using System.Collections.ObjectModel;
using System.Linq;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using BaconGameJam.Common.Models.Doodads;

namespace BaconGameJam.Common.Models.Levels
{
    public class Level
    {
        private const float WorldStep = 1f / 60f;

        private readonly World world;
        private readonly Collection<IDoodad> doodads;

        public Level(World world, Collection<IDoodad> doodads)
        {
            this.world = world;
            this.doodads = doodads;
        }

        public bool LevelCleared { get; private set; }
        public bool LevelLost { get; private set; }

        public void Update(GameTime gameTime)
        {
            world.Step(Level.WorldStep);

            var doodads = this.doodads.ToArray();
            foreach (IDoodad doodad in doodads)
            {
                doodad.Update(gameTime);
            }

            this.LevelCleared = !this.doodads.Any(doodad => doodad is ComputerControlledTank);
            this.LevelLost = !this.doodads.Any(doodad => doodad is PlayerControlledTank);
        }
    }
}