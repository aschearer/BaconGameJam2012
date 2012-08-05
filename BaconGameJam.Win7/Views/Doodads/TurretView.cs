using System;
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

        public int Layer
        {
            get { return 20; }
        }

        public void LoadContent(ContentManager content)
        {
            string textureName = string.Format("Images/InGame/{0}Tank", this.tank.Team);
            this.texture = content.Load<Texture2D>(textureName);
            this.origin = new Vector2(25, 25);
            this.source = new Rectangle(2 * 50, 0, 50, 50);
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
                1,
                SpriteEffects.None,
                0);
        }

        public void Dispose()
        {
        }
    }
}