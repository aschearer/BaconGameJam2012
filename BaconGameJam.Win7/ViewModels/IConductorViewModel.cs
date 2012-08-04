using System;

namespace BaconGameJam.Win7.ViewModels
{
    public interface IConductorViewModel
    {
        event EventHandler<NavigationEventArgs> PushViewModel;
        event EventHandler<EventArgs> PopViewModel;
        void Push(Type viewModel);
        void Pop();
    }
}