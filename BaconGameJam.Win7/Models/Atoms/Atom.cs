using Microsoft.Xna.Framework;

namespace BaconGameJam.Win7.Models.Atoms
{
    public class Atom
    {
        private readonly int positiveCharge;

        public Atom(Vector2 position, int positiveCharge)
        {
            this.Position = position;
            this.positiveCharge = positiveCharge;
        }

        public Vector2 Position { get; private set; }
    }
}