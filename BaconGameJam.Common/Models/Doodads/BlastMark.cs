using System;
using Microsoft.Xna.Framework;

namespace BaconGameJam.Common.Models.Doodads
{
    public class BlastMark : IStaticDoodad
    {
        public BlastMark(Vector2 position, Random random)
        {
            this.Position = position;
            this.Rotation = (float)(MathHelper.TwoPi * random.NextDouble());
            this.BlastId = random.Next(4);
        }

        public bool IsSmall { get; set; }

        public void RemoveFromGame()
        {
        }

        public void Update(GameTime gameTime)
        {
        }

        public Vector2 Position { get; private set; }

        public Rectangle? Source
        {
            get { throw new NotImplementedException(); }
        }

        public float Rotation { get; private set; }

        public int BlastId { get; private set; }
    }
}