using System;
using System.Collections.Generic;
using System.Linq;
using BaconGameJam.Win7.ViewModels;
using Microsoft.Xna.Framework;

namespace BaconGameJam.Win7.Views
{
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