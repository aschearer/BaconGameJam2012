using System;
using System.Collections.Generic;
using BaconGameJam.Win7.Models.Atoms;
using BaconGameJam.Win7.Models.Garden;
using Microsoft.Xna.Framework;

namespace BaconGameJam.Win7.ViewModels.States
{
    public class PlayingViewModel : ViewModelBase
    {
        private readonly List<Flower> flowers;

        public PlayingViewModel()
        {
            this.flowers = new List<Flower>();
            this.flowers.Add(new Flower(new Vector2(400, 240)));
        }

        public IEnumerable<Flower> Flowers
        {
            get { return this.flowers; }
        }

        public void Update(GameTime gameTime)
        {
            foreach (Flower atom in this.flowers)
            {
                atom.Update(gameTime);
            }
        }
    }
}