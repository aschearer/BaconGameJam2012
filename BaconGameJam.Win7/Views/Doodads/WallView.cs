using System;
using BaconGameJam.Common;
using BaconGameJam.Common.Models.Levels;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace BaconGameJam.Win7.Views.Doodads
{
    public class WallView : IRetainedControl
    {
        private readonly Wall wall;
        private Texture2D texture;
        private Vector2 origin;

        public WallView(Wall wall)
        {
            this.wall = wall;
        }

        public void LoadContent(ContentManager content)
        {
            this.texture = content.Load<Texture2D>("Levels/CompoundTileSet");
            this.origin = new Vector2(20, 20);
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
                this.texture,
                wall.Position * Constants.PixelsPerMeter,
                wall.Source,
                Color.White,
                wall.Rotation,
                this.origin,
                1,
                SpriteEffects.None,
                0);
        }
    }
}