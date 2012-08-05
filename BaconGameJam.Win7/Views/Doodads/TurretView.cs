using BaconGameJam.Common;
using BaconGameJam.Common.Models.Doodads;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace BaconGameJam.Win7.Views.Doodads
{
    public class TurretView : IRetainedControl
    {
        private readonly Tank tank;
        private Texture2D texture;
        private Vector2 origin;
        private Rectangle source;

        public TurretView(Tank tank)
        {
            this.tank = tank;
        }

        public void LoadContent(ContentManager content)
        {
            this.texture = content.Load<Texture2D>("Images/InGame/Tanks");
            this.origin = new Vector2(16, 16);
            this.source = new Rectangle(0, 0, 32, 32);
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
                this.texture,
                this.tank.Position * Constants.PixelsPerMeter,
                this.source,
                Color.White,
                this.tank.Heading,
                this.origin,
                2,
                SpriteEffects.None,
                0);
        }
    }
}