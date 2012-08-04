using BaconGameJam.Common.Models.Doodads;
using BaconGameJam.Common.Models.Levels;

namespace BaconGameJam.Win7.Views.Doodads
{
    public class DoodadViewFactory
    {
        public IRetainedControl CreateViewFor(IDoodad doodad)
        {
            if (doodad is Wall)
            {
                return new WallView((Wall)doodad);
            }
            else
            {
                return new TankView((Tank)doodad);
            }
        }
    }
}