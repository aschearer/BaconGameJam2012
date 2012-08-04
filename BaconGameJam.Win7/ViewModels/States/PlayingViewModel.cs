using System.Collections.ObjectModel;
using BaconGameJam.Common.Models.Doodads;
using BaconGameJam.Common.Models.Levels;
using Microsoft.Xna.Framework;

namespace BaconGameJam.Win7.ViewModels.States
{
    public class PlayingViewModel : ViewModelBase
    {
        private readonly LevelFactory levelFactory;
        private readonly Level level;
        private readonly ObservableCollection<IDoodad> doodads;

        public PlayingViewModel(
            LevelFactory levelFactory, 
            Level level,
            ObservableCollection<IDoodad> doodads)
        {
            this.levelFactory = levelFactory;
            this.level = level;
            this.doodads = doodads;
        }

        public ObservableCollection<IDoodad> Tanks
        {
            get { return this.doodads; }
        }

        public void NavigateTo()
        {
            this.levelFactory.LoadLevel();
        }

        public void Update(GameTime gameTime)
        {
            this.level.Update(gameTime);
        }
    }
}