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