using System;

namespace BaconGameJam.Win7.ViewModels.States
{
    public class CreditsViewModel : ViewModelBase
    {
        private readonly IConductorViewModel conductor;

        public CreditsViewModel(IConductorViewModel conductor)
        {
            this.conductor = conductor;
        }

        public void GoToTitle()
        {
            this.conductor.SetTop(typeof(TitleViewModel));
        }
    }
}