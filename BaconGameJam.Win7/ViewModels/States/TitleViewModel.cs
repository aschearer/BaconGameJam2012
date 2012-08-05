using System;
using System.Windows.Input;
using BaconGameJam.Common.Models.Levels;
using GalaSoft.MvvmLight.Command;

namespace BaconGameJam.Win7.ViewModels.States
{
    public class TitleViewModel
    {
        private readonly IConductorViewModel conductor;
        private readonly LevelFactory levelFactory;

        public TitleViewModel(IConductorViewModel conductor, LevelFactory levelFactory)
        {
            this.conductor = conductor;
            this.levelFactory = levelFactory;
            this.StartGameCommand = new RelayCommand(this.StartGame);
            this.ShowCreditsCommand = new RelayCommand(this.ShowCredits);
        }

        public ICommand StartGameCommand { get; private set; }

        public ICommand ShowCreditsCommand { get; private set; }

        public void StartGame()
        {
            this.levelFactory.Reset();
            this.conductor.Push(typeof(PlayingViewModel));
        }

        private void ShowCredits()
        {
            this.conductor.Push(typeof(CreditsViewModel));
        }
    }
}