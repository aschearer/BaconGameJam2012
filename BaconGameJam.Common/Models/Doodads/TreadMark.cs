using System;
using System.Collections.ObjectModel;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;

namespace BaconGameJam.Common.Models.Doodads
{
    public class TreadMark : IStaticDoodad
    {
        private TimeSpan timer;
        private readonly Collection<IDoodad> doodads;

        public TreadMark(Vector2 position, float rotation, Collection<IDoodad> doodads)
        {
            this.Position = position;
            this.Rotation = rotation;
            this.doodads = doodads;
        }

        public void RemoveFromGame()
        {
        }

        public void Update(GameTime gameTime)
        {
            this.timer += gameTime.ElapsedGameTime;
            if (this.timer.TotalSeconds > 10)
            {
                this.doodads.Remove(this);
            }
        }

        public Vector2 Position { get; private set; }

        public float Alpha
        {
            get { return (float)((10 - this.timer.TotalSeconds) / 10); }
        }

        public Rectangle? Source
        {
            get { throw new NotImplementedException(); }
        }

        public float Rotation { get; private set; }
    }
}