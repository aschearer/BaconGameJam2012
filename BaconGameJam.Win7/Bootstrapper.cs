using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using BaconGameJam.Common.Models.Doodads;
using BaconGameJam.Common.Models.Levels;
using BaconGameJam.Common.Models.Sessions;
using BaconGameJam.Common.Models.Sounds;
using BaconGameJam.Win7.ViewModels;
using BaconGameJam.Win7.ViewModels.States;
using BaconGameJam.Win7.Views;
using BaconGameJam.Win7.Views.Doodads;
using BaconGameJam.Win7.Views.Farseer;
using BaconGameJam.Win7.Views.Input;
using BaconGameJam.Win7.Views.Levels;
using BaconGameJam.Win7.Views.Sounds;
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
            Collection<Waypoint> waypoints = new Collection<Waypoint>();
            ObservableCollection<IDoodad> doodads = new ObservableCollection<IDoodad>();
            SimpleIoc.Default.Register(() => doodads);
            SimpleIoc.Default.Register<Collection<IDoodad>>(() => doodads);
            SimpleIoc.Default.Register(() => waypoints);
            SimpleIoc.Default.Register<IEnumerable<IDoodad>>(() => waypoints);
            SimpleIoc.Default.Register(() => new World(Vector2.Zero));
            SimpleIoc.Default.Register<DoodadFactory>();
            SimpleIoc.Default.Register<Level>();
            SimpleIoc.Default.Register<LevelFactory>();
            SimpleIoc.Default.Register<GameSettings>();
            SimpleIoc.Default.Register<ISoundManager, SoundManager>();

            // View Models
            SimpleIoc.Default.Register<IConductorViewModel, ConductorViewModel>();
            SimpleIoc.Default.Register<PlayingViewModel>();
            SimpleIoc.Default.Register<LevelCompleteViewModel>();
            SimpleIoc.Default.Register<GameOverViewModel>();
            SimpleIoc.Default.Register<CreditsViewModel>();
            SimpleIoc.Default.Register<TitleViewModel>();

            // Views
            SimpleIoc.Default.Register<IInputManager, MouseInputManager>();
            SimpleIoc.Default.Register(() => (MouseInputManager)this.GetInstance<IInputManager>());
            SimpleIoc.Default.Register<IKeyboardInputManager, KeyboardInputManager>();
            SimpleIoc.Default.Register(() => (KeyboardInputManager)this.GetInstance<IKeyboardInputManager>());
            SimpleIoc.Default.Register<ConductorView>();
            SimpleIoc.Default.Register<PlayingView>();
            SimpleIoc.Default.Register<GameOverView>();
            SimpleIoc.Default.Register<LevelView>();
            SimpleIoc.Default.Register<DoodadViewFactory>();
            SimpleIoc.Default.Register<DebugViewXNA>();
            SimpleIoc.Default.Register<SoundManagerView>();
            SimpleIoc.Default.Register<LevelCompleteView>();
            SimpleIoc.Default.Register<CreditsView>();
            SimpleIoc.Default.Register<TitleView>();

            List<IScreenView> screenViews = new List<IScreenView>();
            screenViews.Add(this.GetInstance<PlayingView>());
            screenViews.Add(this.GetInstance<GameOverView>());
            screenViews.Add(this.GetInstance<LevelCompleteView>());
            screenViews.Add(this.GetInstance<CreditsView>());
            screenViews.Add(this.GetInstance<TitleView>());
            SimpleIoc.Default.Register<IEnumerable<IScreenView>>(() => screenViews);
        }

        public T GetInstance<T>()
        {
            return (T)SimpleIoc.Default.GetInstance(typeof(T));
        }
    }
}