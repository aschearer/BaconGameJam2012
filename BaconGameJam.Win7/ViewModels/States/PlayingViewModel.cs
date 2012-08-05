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
        private readonly IConductorViewModel conductor;
        private bool GameOver;

        public PlayingViewModel(
            LevelFactory levelFactory, 
            Level level,
            ObservableCollection<IDoodad> doodads,
            IConductorViewModel conductor)
        {
            this.levelFactory = levelFactory;
            this.level = level;
            this.doodads = doodads;
            this.conductor = conductor;
            this.GameOver = false;
        }

        public ObservableCollection<IDoodad> Tanks
        {
            get { return this.doodads; }
        }

        public void NavigateTo()
        {
            this.GameOver = false;
            this.levelFactory.LoadLevel();
        }

        public void Update(GameTime gameTime)
        {
            this.level.Update(gameTime);
            if (this.level.GameOver)
            {
                if (!this.GameOver)
                {
                    this.GameOver = true;
                    this.conductor.Push(typeof(GameOverViewModel));
                }
            }
        }
    }
}