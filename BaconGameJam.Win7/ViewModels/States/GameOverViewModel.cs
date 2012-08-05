using System.Collections.ObjectModel;
using BaconGameJam.Common.Models.Doodads;
using BaconGameJam.Common.Models.Levels;
using Microsoft.Xna.Framework;

namespace BaconGameJam.Win7.ViewModels.States
{
    public class GameOverViewModel : ViewModelBase
    {
        private readonly LevelFactory levelFactory;
        private readonly Level level;

        public GameOverViewModel(
            LevelFactory levelFactory, 
            Level level)
        {
            this.levelFactory = levelFactory;
            this.level = level;
        }

        public void NavigateTo()
        {
            
        }

        public void Update(GameTime gameTime)
        {
            this.level.Update(gameTime);
        }
    }
}