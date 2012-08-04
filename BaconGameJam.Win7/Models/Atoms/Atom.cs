using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace BaconGameJam.Win7.Models.Atoms
{
    public class Atom
    {
        public const float Radius = 50;

        private readonly int positiveCharge;
        private readonly List<Electron> electrons;

        public Atom(Vector2 position, int positiveCharge, int negativeCharge)
        {
            this.Position = position;
            this.positiveCharge = positiveCharge;
            this.electrons = new List<Electron>();
            for (int i = 0; i < negativeCharge; i++)
            {
                this.electrons.Add(new Electron(this, 1));
            }
        }

        public Vector2 Position { get; private set; }

        public IEnumerable<Electron> Electrons
        {
            get { return this.electrons; }
        }

        public void Update(GameTime gameTime)
        {
            foreach (Electron electron in this.electrons)
            {
                electron.Update(gameTime);
            }
        }
    }
}