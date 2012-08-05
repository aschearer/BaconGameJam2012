using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;
using BaconGameJam.Common.Models.Doodads;
using Microsoft.Xna.Framework.Content;

namespace BaconGameJam.Common.Models.Levels
{
    public class LevelFactory
    {
        private readonly ContentManager content;
        private readonly DoodadFactory doodadFactory;
        private readonly Collection<IDoodad> doodads;

        public LevelFactory(ContentManager content, DoodadFactory doodadFactory, Collection<IDoodad> doodads)
        {
            this.doodadFactory = doodadFactory;
            this.doodads = doodads;
            this.content = content;
        }

        public void LoadLevel()
        {
            var doodads = this.doodads.ToArray();
            foreach (IDoodad doodad in doodads)
            {
                doodad.RemoveFromGame();
            }

            var doodadPlacements = this.content.Load<IEnumerable<DoodadPlacement>>("Levels/TestLevel");
            foreach (DoodadPlacement doodadPlacement in doodadPlacements)
            {
                this.doodadFactory.CreateDoodad(doodadPlacement);
            }
        }
    }
}