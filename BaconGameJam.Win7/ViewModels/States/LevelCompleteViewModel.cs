using System.Windows.Input;
using BaconGameJam.Common.Models.Levels;
using GalaSoft.MvvmLight.Command;
using Microsoft.Xna.Framework;

namespace BaconGameJam.Win7.ViewModels.States
{
    public class LevelCompleteViewModel
    {
        private readonly Level level;
        private readonly LevelFactory levelFactory;
        private readonly IConductorViewModel conductor;

        public LevelCompleteViewModel(
            Level level, 
            LevelFactory levelFactory,
            IConductorViewModel conductor)
        {
            this.level = level;
            this.levelFactory = levelFactory;
            this.conductor = conductor;
            this.NextLevelCommand = new RelayCommand(this.StartNewGame);
        }

        public ICommand NextLevelCommand { get; private set; }

        private void StartNewGame()
        {
            this.levelFactory.LoadNextLevel();
            this.conductor.Pop();
        }

        public void Update(GameTime gameTime)
        {
            this.level.Update(gameTime);
        }
    }
}