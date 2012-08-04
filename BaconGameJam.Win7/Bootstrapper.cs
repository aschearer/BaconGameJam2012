using System;
using System.Collections.Generic;
using BaconGameJam.Win7.Models.Tanks;
using BaconGameJam.Win7.ViewModels;
using BaconGameJam.Win7.ViewModels.States;
using BaconGameJam.Win7.Views;
using BaconGameJam.Win7.Views.Garden;
using BaconGameJam.Win7.Views.Input;
using BaconGameJam.Win7.Views.States;
using BaconGameJam.Win7.Views.Tanks;
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
            SimpleIoc.Default.Register(() => new World(Vector2.Zero));
            SimpleIoc.Default.Register<TankFactory>();

            // View Models
            SimpleIoc.Default.Register<IConductorViewModel, ConductorViewModel>();
            SimpleIoc.Default.Register<PlayingViewModel>();

            // Views
            SimpleIoc.Default.Register<IInputManager, MouseInputManager>();
            SimpleIoc.Default.Register(() => (MouseInputManager)this.GetInstance<IInputManager>());
            SimpleIoc.Default.Register<ConductorView>();
            SimpleIoc.Default.Register<PlayingView>();
            SimpleIoc.Default.Register<FlowerView>();
            SimpleIoc.Default.Register<TankView>();

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