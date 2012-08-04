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
            float startingTheta = 0;
            int shell = 1;
            int electronsInShell = 0;
            for (int i = 0; i < negativeCharge; i++)
            {
                electronsInShell++;
                this.electrons.Add(new Electron(this, shell, startingTheta));
                startingTheta += MathHelper.TwoPi / this.MaxElectronsForShell(shell);
                if (electronsInShell == this.MaxElectronsForShell(shell))
                {
                    electronsInShell = 0;
                    shell++;
                    startingTheta += MathHelper.PiOver2;
                }
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

        private int MaxElectronsForShell(int shell)
        {
            return shell + 1;
        }
    }
}