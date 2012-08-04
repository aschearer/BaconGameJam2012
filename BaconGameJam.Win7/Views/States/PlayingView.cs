using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using BaconGameJam.Common;
using BaconGameJam.Common.Models.Doodads;
using BaconGameJam.Win7.ViewModels.States;
using BaconGameJam.Win7.Views.Doodads;
using BaconGameJam.Win7.Views.Farseer;
using BaconGameJam.Win7.Views.Levels;
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
        private readonly DoodadViewFactory doodadViewFactory;
        private readonly LevelView levelView;
        private readonly DebugViewXNA debugView;
        private readonly List<IRetainedControl> doodadViews;
        private bool isContentLoaded;

        public PlayingView(
            ContentManager content, 
            SpriteBatch spriteBatch,
            PlayingViewModel viewModel,
            DoodadViewFactory doodadViewFactory,
            LevelView levelView,
            DebugViewXNA debugView)
        {
            this.content = content;
            this.doodadViews = new List<IRetainedControl>();
            this.spriteBatch = spriteBatch;
            this.viewModel = viewModel;
            this.doodadViewFactory = doodadViewFactory;
            this.levelView = levelView;
            this.viewModel.Tanks.CollectionChanged += this.OnTanksChanged;

            this.debugView = debugView;
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
            this.levelView.Draw(gameTime, spriteBatch);
            foreach (IRetainedControl doodadView in this.doodadViews)
            {
                doodadView.Draw(gameTime, this.spriteBatch);
            }

            this.spriteBatch.End();

            if (Constants.Debug)
            {
                var matrix = Matrix.CreateOrthographicOffCenter(0f,
                                            800 / Constants.PixelsPerMeter,
                                            480 / Constants.PixelsPerMeter,
                                            0f,
                                            0f,
                                            1f);

                this.debugView.RenderDebugData(ref matrix);
            }
        }

        private void LoadContent()
        {
            if (this.isContentLoaded)
            {
                return;
            }

            this.isContentLoaded = true;
            foreach (IRetainedControl doodadView in this.doodadViews)
            {
                doodadView.LoadContent(this.content);
            }

            this.levelView.LoadContent(this.content);
            this.debugView.LoadContent(this.spriteBatch.GraphicsDevice, this.content);
        }

        private void OnTanksChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    foreach (IDoodad doodad in e.NewItems)
                    {
                        this.doodadViews.Add(this.doodadViewFactory.CreateViewFor(doodad));
                        if (this.isContentLoaded)
                        {
                            this.doodadViews.Last().LoadContent(this.content);
                        }
                    }

                    break;
                case NotifyCollectionChangedAction.Remove:
                    for (int i = e.OldStartingIndex + e.OldItems.Count - 1; i >= e.OldStartingIndex; i--)
                    {
                        this.doodadViews.RemoveAt(i);
                    }

                    break;
                case NotifyCollectionChangedAction.Reset:
                    this.doodadViews.Clear();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}