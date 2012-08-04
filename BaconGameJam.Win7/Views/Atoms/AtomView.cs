using BaconGameJam.Win7.Models.Atoms;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace BaconGameJam.Win7.Views.Atoms
{
    public class AtomView : IControl<Atom>
    {
        private ElectronView electronView;
        private Texture2D texture;
        private Vector2 origin;

        public AtomView(ElectronView electronView)
        {
            this.electronView = electronView;
        }

        public void LoadContent(ContentManager content)
        {
            this.electronView.LoadContent(content);
            this.texture = content.Load<Texture2D>("Images/InGame/Atom");
            this.origin = new Vector2(this.texture.Width / 2f, this.texture.Height / 2f);
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, Atom dataContext)
        {
            spriteBatch.Draw(
                this.texture,
                dataContext.Position,
                null,
                Color.White,
                0,
                this.origin,
                1,
                SpriteEffects.None,
                0);

            foreach (Electron electron in dataContext.Electrons)
            {
                this.electronView.Draw(gameTime, spriteBatch, electron);
            }
        }
    }
}