using System;
using System.Collections.Generic;
using System.Linq;
using BaconGameJam.Win7.ViewModels;
using Microsoft.Xna.Framework;

namespace BaconGameJam.Win7.Views
{
    /// <summary>
    /// Responsible for managing the various visual game states. Drives the 
    /// game's logic through it's Draw and Update methods.
    /// </summary>
    /// <remarks>
    /// This class organizes the visual states in the game. Each state is mutually 
    /// exclusive, so you cannot be in both the "Playing" state and the "Game Over"
    /// state. (To change this simply change how the list of active views are drawn
    /// and updated.)
    /// </remarks>
    /// <remarks>
    /// This class builds it's list of active views by listening for navigation
    /// events from the ConductorViewModel. Whenever the view model changes state
    /// the ConductorView tries to find the corresponding view and add it to the
    /// stack of active views.
    /// </remarks>
    class ConductorView
    {
        private readonly IConductorViewModel viewModel;
        private readonly IEnumerable<IScreenView> views;
        private readonly Stack<IScreenView> activeViews;

        public ConductorView(
            IConductorViewModel viewModel, 
            IEnumerable<IScreenView> views)
        {
            this.viewModel = viewModel;
            this.views = views;
            this.activeViews = new Stack<IScreenView>();
            this.viewModel.PushViewModel += this.OnViewModelPushed;
            this.viewModel.PopViewModel += this.OnViewModelPopped;
            this.viewModel.SetTopViewModel += this.OnViewModelSetTop;
        }

        public void Update(GameTime gameTime)
        {
            if (this.activeViews.Count > 0)
            {
                this.activeViews.Peek().Update(gameTime);
            }
        }

        public void Draw(GameTime gameTime)
        {
            if (this.activeViews.Count > 0)
            {
                this.activeViews.Peek().Draw(gameTime);
            }
        }

        private void OnViewModelSetTop(object sender, NavigationEventArgs e)
        {
            while (this.activeViews.Count > 0)
            {
                this.activeViews.Peek().NavigateFrom();
                this.activeViews.Pop();
            }

            this.activeViews.Push(this.GetViewForViewModel(e.TargetViewModel));
            this.activeViews.Peek().NavigateTo();
        }

        private void OnViewModelPushed(object sender, NavigationEventArgs e)
        {
            if (this.activeViews.Count > 0)
            {
                this.activeViews.Peek().NavigateFrom();
            }

            this.activeViews.Push(this.GetViewForViewModel(e.TargetViewModel));
            this.activeViews.Peek().NavigateTo();
        }

        private void OnViewModelPopped(object sender, EventArgs e)
        {
            if (this.activeViews.Count > 0)
            {
                this.activeViews.Peek().NavigateFrom();
            }

            this.activeViews.Pop();
            this.activeViews.Peek().NavigateTo();
        }

        private IScreenView GetViewForViewModel(Type currentViewModel)
        {
            string viewName = currentViewModel.Name.Substring(0, currentViewModel.Name.Length - 5);
            return this.views.FirstOrDefault(view => view.GetType().Name == viewName);
        }
    }
}