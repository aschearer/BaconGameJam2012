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
        private readonly Level level;
        private Texture2D startingTileSet;
        private Vector2 origin;
        private Texture2D endingTileSet;

        public StaticDoodadView(IStaticDoodad doodad, Level level)
        {
            this.doodad = doodad;
            this.level = level;
        }

        public int Layer
        {
            get { return 0; }
        }

        public void LoadContent(ContentManager content)
        {
            this.startingTileSet = content.Load<Texture2D>("TileSets/ConcreteStormTileSet");
            this.endingTileSet = content.Load<Texture2D>("TileSets/TanksTiled");
            this.origin = new Vector2(16, 16);
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
                level.Number < 6 ? this.startingTileSet : this.endingTileSet,
                this.doodad.Position * Constants.PixelsPerMeter,
                this.doodad.Source,
                Color.White,
                this.doodad.Rotation,
                this.origin,
                1,
                SpriteEffects.None,
                0);
        }

        public void Dispose()
        {
        }
    }
}