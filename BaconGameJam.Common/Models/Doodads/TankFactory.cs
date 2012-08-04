using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;

namespace BaconGameJam.Common.Models.Doodads
{
    public class TankFactory
    {
        private readonly World world;

        public TankFactory(World world)
        {
            this.world = world;
        }

        public Tank CreateTank(Team team, Vector2 position, float rotation)
        {
            return new Tank(this.world, team, position, rotation);
        }
    }
}