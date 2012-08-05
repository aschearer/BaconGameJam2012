using System;
using BaconGameJam.Common;
using BaconGameJam.Common.Models.Doodads;
using BaconGameJam.Common.Models.Levels;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace BaconGameJam.Win7.Views.Doodads
{
    public class StaticDoodadView : IRetainedControl
    {
        private readonly IStaticDoodad doodad;
        private Texture2D texture;
        private Vector2 origin;

        public StaticDoodadView(IStaticDoodad doodad)
        {
            this.doodad = doodad;
        }

        public void LoadContent(ContentManager content)
        {
            this.texture = content.Load<Texture2D>("TileSets/ConcreteStormTileSet");
            this.origin = new Vector2(16, 16);
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
                this.texture,
                this.doodad.Position * Constants.PixelsPerMeter,
                this.doodad.Source,
                Color.White,
                this.doodad.Rotation,
                this.origin,
                2,
                SpriteEffects.None,
                0);
        }
    }
}