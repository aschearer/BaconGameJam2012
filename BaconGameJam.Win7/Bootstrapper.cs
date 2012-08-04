using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using BaconGameJam.Common.Models.Doodads;
using BaconGameJam.Common.Models.Levels;
using BaconGameJam.Win7.ViewModels;
using BaconGameJam.Win7.ViewModels.States;
using BaconGameJam.Win7.Views;
using BaconGameJam.Win7.Views.Doodads;
using BaconGameJam.Win7.Views.Farseer;
using BaconGameJam.Win7.Views.Input;
using BaconGameJam.Win7.Views.Levels;
using BaconGameJam.Win7.Views.States;
using FarseerPhysics.Dynamics;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace BaconGameJam.Win7
{
    public class Bootstrapper
    {
        public Bootstrapper(ContentManager content, SpriteBatch spriteBatch)
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            // Framework
            SimpleIoc.Default.Register(() => content);
            SimpleIoc.Default.Register(() => spriteBatch);
            SimpleIoc.Default.Register(() => new Random());

            // Models
            ObservableCollection<IDoodad> doodads = new ObservableCollection<IDoodad>();
            SimpleIoc.Default.Register(() => doodads);
            SimpleIoc.Default.Register<Collection<IDoodad>>(() => doodads);
            SimpleIoc.Default.Register(() => new World(Vector2.Zero));
            SimpleIoc.Default.Register<DoodadFactory>();
            SimpleIoc.Default.Register<Level>();
            SimpleIoc.Default.Register<LevelFactory>();

            // View Models
            SimpleIoc.Default.Register<IConductorViewModel, ConductorViewModel>();
            SimpleIoc.Default.Register<PlayingViewModel>();

            // Views
            SimpleIoc.Default.Register<IInputManager, MouseInputManager>();
            SimpleIoc.Default.Register(() => (MouseInputManager)this.GetInstance<IInputManager>());
            SimpleIoc.Default.Register<ConductorView>();
            SimpleIoc.Default.Register<PlayingView>();
            SimpleIoc.Default.Register<LevelView>();
            SimpleIoc.Default.Register<DoodadViewFactory>();
            SimpleIoc.Default.Register<DebugViewXNA>();

            List<IScreenView> screenViews = new List<IScreenView>();
            screenViews.Add(this.GetInstance<PlayingView>());
            SimpleIoc.Default.Register<IEnumerable<IScreenView>>(() => screenViews);
        }

        public T GetInstance<T>()
        {
            return (T)SimpleIoc.Default.GetInstance(typeof(T));
        }
    }
}