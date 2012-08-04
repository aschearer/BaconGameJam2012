using System.Collections.ObjectModel;
using BaconGameJam.Win7.Models.Tanks;
using Microsoft.Xna.Framework;

namespace BaconGameJam.Win7.ViewModels.States
{
    public class PlayingViewModel : ViewModelBase
    {
        private readonly TankFactory tankFactory;
        private readonly ObservableCollection<Tank> tanks;

        public PlayingViewModel(TankFactory tankFactory)
        {
            this.tankFactory = tankFactory;
            this.tanks = new ObservableCollection<Tank>();
        }

        public ObservableCollection<Tank> Tanks
        {
            get { return this.tanks; }
        }

        public void NavigateTo()
        {
            int x = 200;
            for (int i = 0; i < 5; i++)
            {
                this.tanks.Add(this.tankFactory.CreateTank(Team.Red, new Vector2(x, 180), MathHelper.Pi));
                this.tanks.Add(this.tankFactory.CreateTank(Team.Green, new Vector2(x, 300), 0));
                x += 100;
            }
        }

        public void Update(GameTime gameTime)
        {
            foreach (Tank tank in this.tanks)
            {
                tank.Update(gameTime);
            }
        }
    }
}