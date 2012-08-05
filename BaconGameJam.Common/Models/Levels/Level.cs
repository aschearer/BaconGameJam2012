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
        private Collection<IDoodad> doodads;
        public bool GameOver { get; protected set; }

        public Level(World world, Collection<IDoodad> doodads)
        {
            this.world = world;
            this.doodads = doodads;
        }

        public void Update(GameTime gameTime)
        {
            world.Step(Level.WorldStep);

            if (!GameOver)
            {
                bool playerIsAlive = this.doodads.Any(doodad => doodad is PlayerControlledTank);
                bool enemiesAlive = this.doodads.Any(doodad => doodad is ComputerControlledTank);
                if (!playerIsAlive) this.GameOver = true;
                else if (!enemiesAlive) this.GameOver = true;

                var doodads = this.doodads.ToArray();
                foreach (IDoodad doodad in doodads)
                {
                    doodad.Update(gameTime);
                }
            }

            //world.Step(Level.WorldStep);
        }
    }
}