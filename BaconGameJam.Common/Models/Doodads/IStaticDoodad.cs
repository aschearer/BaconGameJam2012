using Microsoft.Xna.Framework;

namespace BaconGameJam.Common.Models.Doodads
{
    public interface IStaticDoodad : IDoodad
    {
        Vector2 Position { get; }
        Rectangle? Source { get; }
        float Rotation { get; }
    }
}