using System.Collections.ObjectModel;
using BaconGameJam.Common.Models.Doodads;
using BaconGameJam.Common.Models.Levels;
using BaconGameJam.Common.Models.Sounds;
using Microsoft.Xna.Framework;

namespace BaconGameJam.Win7.ViewModels.States
{
    public class PlayingViewModel : ViewModelBase
    {
        private readonly LevelFactory levelFactory;
        private readonly Level level;
        private readonly ObservableCollection<IDoodad> doodads;
        private readonly IConductorViewModel conductor;
        private int currentLevel;

        public PlayingViewModel(
            LevelFactory levelFactory, 
            Level level,
            ObservableCollection<IDoodad> doodads,
            IConductorViewModel conductor)
        {
            this.currentLevel = 1;
            this.levelFactory = levelFactory;
            this.level = level;
            this.doodads = doodads;
            this.conductor = conductor;
        }

        public ObservableCollection<IDoodad> Tanks
        {
            get { return this.doodads; }
        }

        public void NavigateTo()
        {
            this.levelFactory.LoadLevel(this.currentLevel);
        }

        public void Update(GameTime gameTime)
        {
            this.level.Update(gameTime);
            if (this.level.LevelCleared || this.level.LevelLost)
            {
                this.conductor.Push(typeof(GameOverViewModel));
            }
        }
    }
}