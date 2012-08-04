using System;
using BaconGameJam.Common;
using BaconGameJam.Common.Models.Doodads;
using BaconGameJam.Win7.Views.Input;
using Microsoft.Xna.Framework;

namespace BaconGameJam.Win7.Views.Doodads
{
    public class PlayerControlledTankView : TankView
    {
        private readonly PlayerControlledTank tank;
        private readonly IInputManager input;

        public PlayerControlledTankView(PlayerControlledTank tank, IInputManager input)
            : base(tank)
        {
            this.tank = tank;
            this.input = input;
            this.input.Click += this.OnClick;
        }

        private void OnClick(object sender, InputEventArgs e)
        {
            this.tank.FireMissileCommand.Execute(new Vector2(e.X, e.Y) / Constants.PixelsPerMeter);
        }
    }
}