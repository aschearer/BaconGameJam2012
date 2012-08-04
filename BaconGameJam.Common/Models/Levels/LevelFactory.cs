using System.Collections.Generic;
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
            foreach (IDoodad doodad in this.doodads)
            {
                doodad.RemoveFromGame();
            }

            var doodadPlacements = this.content.Load<IEnumerable<DoodadPlacement>>("Levels/Level1");
            foreach (DoodadPlacement doodadPlacement in doodadPlacements)
            {
                this.doodads.Add(this.doodadFactory.CreateDoodad(doodadPlacement));
            }
        }
    }
}