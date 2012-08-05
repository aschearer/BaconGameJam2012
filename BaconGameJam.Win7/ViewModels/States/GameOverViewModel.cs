using System.Windows.Input;
using BaconGameJam.Common.Models.Levels;
using GalaSoft.MvvmLight.Command;
using Microsoft.Xna.Framework;

namespace BaconGameJam.Win7.ViewModels.States
{
    public class GameOverViewModel : ViewModelBase
    {
        private readonly Level level;
        private readonly IConductorViewModel conductor;

        public GameOverViewModel(
            Level level, 
            IConductorViewModel conductor)
        {
            this.level = level;
            this.conductor = conductor;
            this.NewGameCommand = new RelayCommand(this.StartNewGame);
        }

        public ICommand NewGameCommand { get; private set; }

        private void StartNewGame()
        {
            this.conductor.Pop();
        }

        public void Update(GameTime gameTime)
        {
            this.level.Update(gameTime);
        }
    }
}