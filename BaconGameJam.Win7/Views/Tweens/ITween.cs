using Microsoft.Xna.Framework;

namespace BaconGameJam.Win7.Views.Tweens
{
    public interface ITween
    {
        float Value { get; }
        Repeat Repeats { get; set; }
        bool YoYos { get; set; }

        void Update(GameTime gameTime);
    }
}