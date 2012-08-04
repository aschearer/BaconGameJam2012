using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using BaconGameJam.Win7.Models.Tanks;
using BaconGameJam.Win7.ViewModels.States;
using BaconGameJam.Win7.Views.Tanks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace BaconGameJam.Win7.Views.States
{
    public class PlayingView : IScreenView
    {
        private readonly ContentManager content;
        private readonly SpriteBatch spriteBatch;
        private readonly PlayingViewModel viewModel;
        private readonly List<TankView> tankViews;
        private bool isContentLoaded;

        public PlayingView(
            ContentManager content, 
            SpriteBatch spriteBatch, 
            PlayingViewModel viewModel)
        {
            this.content = content;
            this.tankViews = new List<TankView>();
            this.spriteBatch = spriteBatch;
            this.viewModel = viewModel;
            this.viewModel.Tanks.CollectionChanged += this.OnTanksChanged;
        }

        public void NavigateTo()
        {
            this.viewModel.NavigateTo();
            this.LoadContent();
        }

        public void NavigateFrom()
        {
        }

        public void Update(GameTime gameTime)
        {
            this.viewModel.Update(gameTime);
        }

        public void Draw(GameTime gameTime)
        {
            this.spriteBatch.Begin();
            foreach (TankView tankView in this.tankViews)
            {
                tankView.Draw(gameTime, this.spriteBatch);
            }

            this.spriteBatch.End();
        }

        private void LoadContent()
        {
            if (this.isContentLoaded)
            {
                return;
            }

            this.isContentLoaded = true;
            foreach (TankView tankView in this.tankViews)
            {
                tankView.LoadContent(this.content);
            }
        }

        private void OnTanksChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    foreach (Tank tank in e.NewItems)
                    {
                        this.tankViews.Add(new TankView(tank));
                        if (this.isContentLoaded)
                        {
                            this.tankViews.Last().LoadContent(this.content);
                        }
                    }

                    break;
                case NotifyCollectionChangedAction.Remove:
                    for (int i = e.OldStartingIndex + e.OldItems.Count - 1; i >= e.OldStartingIndex; i--)
                    {
                        this.tankViews.RemoveAt(i);
                    }

                    break;
                case NotifyCollectionChangedAction.Reset:
                    this.tankViews.Clear();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}