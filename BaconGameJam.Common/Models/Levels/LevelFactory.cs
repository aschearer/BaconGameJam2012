using System;
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
        private readonly Level level;
        private int currentLevel;

        public LevelFactory(
            ContentManager content, 
            DoodadFactory doodadFactory, 
            Collection<IDoodad> doodads,
            Level level)
        {
            this.doodadFactory = doodadFactory;
            this.doodads = doodads;
            this.level = level;
            this.content = content;
            this.currentLevel = 1;
        }

        public bool CanLoadNextLevel
        {
            get { return this.currentLevel < Constants.NumberOfLevels; }
        }

        public void LoadLevel()
        {
            level.Number = this.currentLevel;
            var doodads = this.doodads.ToArray();
            foreach (IDoodad doodad in doodads)
            {
                doodad.RemoveFromGame();
            }

            this.doodads.Clear();

            var doodadPlacements = this.content.Load<IEnumerable<DoodadPlacement>>("Levels/Level" + this.currentLevel);
            foreach (DoodadPlacement doodadPlacement in doodadPlacements)
            {
                this.doodadFactory.CreateDoodad(doodadPlacement);
            }
        }

        public bool LoadNextLevel()
        {
            this.currentLevel++;
            this.LoadLevel();
            return true;
        }

        public void Reset()
        {
            this.currentLevel = 1;
        }
    }
}