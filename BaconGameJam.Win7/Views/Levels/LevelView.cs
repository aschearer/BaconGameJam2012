using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using BaconGameJam.Common.Models.Doodads;
using BaconGameJam.Win7.Views.Doodads;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace BaconGameJam.Win7.Views.Levels
{
    public class LevelView
    {
        private readonly List<IRetainedControl> doodadViews;
        private readonly DoodadViewFactory doodadViewFactory;
        private readonly ObservableCollection<IDoodad> doodads;
        private ContentManager content;

        public LevelView(DoodadViewFactory doodadViewFactory, ObservableCollection<IDoodad> doodads)
        {
            this.doodadViewFactory = doodadViewFactory;
            this.doodads = doodads;
            this.doodads.CollectionChanged += this.OnDoodadsChanged;
            this.doodadViews = new List<IRetainedControl>();
        }

        public void LoadContent(ContentManager content)
        {
            this.content = content;
            foreach (IRetainedControl doodadView in this.doodadViews)
            {
                doodadView.LoadContent(content);
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            var layers = this.doodadViews.GroupBy(view => view.Layer).OrderBy(group => group.Key);
            foreach (var layer in layers)
            {
                foreach (var view in layer)
                {
                    view.Draw(gameTime, spriteBatch);
                }
            }
        }

        private void OnDoodadsChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    foreach (IDoodad doodad in e.NewItems)
                    {
                        this.doodadViews.Add(this.doodadViewFactory.CreateViewFor(doodad));
                        if (this.content != null)
                        {
                            this.doodadViews.Last().LoadContent(this.content);
                        }
                    }

                    break;
                case NotifyCollectionChangedAction.Remove:
                    for (int i = e.OldStartingIndex + e.OldItems.Count - 1; i >= e.OldStartingIndex; i--)
                    {
                        this.doodadViews[i].Dispose();
                        this.doodadViews.RemoveAt(i);
                    }

                    break;
                case NotifyCollectionChangedAction.Reset:
                    foreach (IRetainedControl view in this.doodadViews)
                    {
                        view.Dispose();
                    }

                    this.doodadViews.Clear();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}