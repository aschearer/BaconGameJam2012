using System;
using Microsoft.Xna.Framework;

namespace BaconGameJam.Win7.Models.Garden
{
    public class Flower
    {
        private TimeSpan elapsedTime;

        public Flower(Vector2 position)
        {
            this.Position = position;
            this.State = FlowerState.Seed;
        }

        public Vector2 Position { get; private set; }
        public FlowerState State { get; private set; }

        public void Update(GameTime gameTime)
        {
            this.elapsedTime += gameTime.ElapsedGameTime;
            if (this.elapsedTime > TimeSpan.FromSeconds(2))
            {
                this.elapsedTime = TimeSpan.Zero;
                this.State = (FlowerState)(((int)this.State + 8) % 7);
            }
        }
    }
}