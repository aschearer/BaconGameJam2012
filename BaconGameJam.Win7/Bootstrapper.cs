using System.Collections.Generic;
using BaconGameJam.Win7.ViewModels;
using BaconGameJam.Win7.ViewModels.States;
using BaconGameJam.Win7.Views;
using BaconGameJam.Win7.Views.Input;
using BaconGameJam.Win7.Views.States;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;
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

            // Models

            // View Models
            SimpleIoc.Default.Register<IConductorViewModel, ConductorViewModel>();
            SimpleIoc.Default.Register<PlayingViewModel>();

            // Views
            SimpleIoc.Default.Register<IInputManager, MouseInputManager>();
            SimpleIoc.Default.Register(() => (MouseInputManager)this.GetInstance<IInputManager>());
            SimpleIoc.Default.Register<ConductorView>();
            SimpleIoc.Default.Register<PlayingView>();

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