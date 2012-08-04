using System;

namespace BaconGameJam.Win7.ViewModels
{
    public class NavigationEventArgs : EventArgs
    {
        public NavigationEventArgs(Type targetViewModel)
        {
            this.TargetViewModel = targetViewModel;
        }

        public Type TargetViewModel { get; private set; }
    }
}