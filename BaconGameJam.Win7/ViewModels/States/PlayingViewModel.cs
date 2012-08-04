using System;
using System.Collections.Generic;
using BaconGameJam.Win7.Models.Atoms;
using Microsoft.Xna.Framework;

namespace BaconGameJam.Win7.ViewModels.States
{
    public class PlayingViewModel : ViewModelBase
    {
        private readonly List<Atom> atoms;

        public PlayingViewModel()
        {
            this.atoms = new List<Atom>();
            this.atoms.Add(new Atom(new Vector2(400, 240), 4, 1));
        }

        public IEnumerable<Atom> Atoms
        {
            get { return this.atoms; }
        }

        public void Update(GameTime gameTime)
        {
        }
    }
}