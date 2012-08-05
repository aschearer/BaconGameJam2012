using Microsoft.Xna.Framework;

namespace BaconGameJam.Common.Models.Doodads
{
    public class TileDoodad : IStaticDoodad
    {
        private readonly Rectangle? source;

        public TileDoodad(Vector2 position, float rotation, Rectangle source)
        {
            this.Position = position;
            this.Rotation = rotation;
            this.source = source;
        }

        public Vector2 Position { get; private set; }

        public Rectangle? Source
        {
            get { return this.source; }
        }

        public float Rotation { get; private set; }

        public void RemoveFromGame()
        {
        }

        public void Update(GameTime gameTime)
        {
        }
    }
}