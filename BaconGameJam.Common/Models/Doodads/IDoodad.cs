using Microsoft.Xna.Framework;

namespace BaconGameJam.Common.Models.Doodads
{
    public interface IDoodad
    {
        void RemoveFromGame();
        void Update(GameTime gameTime);
    }
}