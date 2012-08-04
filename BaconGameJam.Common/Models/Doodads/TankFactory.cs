using System.Collections.ObjectModel;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;

namespace BaconGameJam.Common.Models.Doodads
{
    public class TankFactory
    {
        private readonly World world;
        private readonly Collection<IDoodad> doodads;

        public TankFactory(World world, Collection<IDoodad> doodads)
        {
            this.world = world;
            this.doodads = doodads;
        }

        public Tank CreateTank(Team team, Vector2 position, float rotation)
        {
            return new Tank(this.world, this.doodads, team, position, rotation);
        }
    }
}