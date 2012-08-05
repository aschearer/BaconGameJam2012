using System;
using BaconGameJam.Common;
using BaconGameJam.Common.Models.Doodads;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace BaconGameJam.Win7.Views.Doodads
{
    public class WaypointView : IRetainedControl
    {
        private readonly Waypoint waypoint;
        private Texture2D texture;
        private Vector2 origin;

        public WaypointView(Waypoint waypoint)
        {
            this.waypoint = waypoint;
        }

        public int Layer
        {
            get { return 30; }
        }

        public void LoadContent(ContentManager content)
        {
            this.texture = content.Load<Texture2D>("Images/InGame/Waypoint");
            this.origin = new Vector2(this.texture.Width / 2f, this.texture.Height / 2f);
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (!Constants.Debug)
            {
                return;
            }

            spriteBatch.Draw(
                this.texture,
                this.waypoint.Position * Constants.PixelsPerMeter,
                null,
                Color.White,
                0,
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