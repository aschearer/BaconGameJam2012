using System;
using BaconGameJam.Win7.Models.Garden;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace BaconGameJam.Win7.Views.Garden
{
    public class FlowerView : IControl<Flower>
    {
        private readonly Random random;
        private Texture2D texture;
        private Vector2 origin;

        public FlowerView(Random random)
        {
            this.random = random;
        }

        public void LoadContent(ContentManager content)
        {
            this.texture = content.Load<Texture2D>("Images/InGame/Flower");
            this.origin = new Vector2(33, 50);
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, Flower dataContext)
        {
            Rectangle source = new Rectangle(66 * this.random.Next(3), (int)dataContext.State * 100, 66, 100);
            spriteBatch.Draw(
                this.texture,
                dataContext.Position,
                source,
                Color.White,
                0,
                this.origin,
                1,
                SpriteEffects.None,
                0);
        }
    }
}