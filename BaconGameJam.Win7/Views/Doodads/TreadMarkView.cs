using System;
using BaconGameJam.Common;
using BaconGameJam.Common.Models.Doodads;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace BaconGameJam.Win7.Views.Doodads
{
    public class TreadMarkView : IRetainedControl
    {
        private readonly TreadMark treadMark;
        private Texture2D texture;
        private Vector2 origin;

        public TreadMarkView(TreadMark treadMark)
        {
            this.treadMark = treadMark;
        }

        public int Layer
        {
            get { return 1; }
        }

        public void LoadContent(ContentManager content)
        {
            this.texture = content.Load<Texture2D>("Images/InGame/TreadMarks");
            this.origin = new Vector2(16, 5);
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
                this.texture,
                this.treadMark.Position * Constants.PixelsPerMeter,
                null,
                Color.White * this.treadMark.Alpha,
                this.treadMark.Rotation,
                this.origin,
                1,
                SpriteEffects.None,
                1);
        }

        public void Dispose()
        {
        }
    }
}