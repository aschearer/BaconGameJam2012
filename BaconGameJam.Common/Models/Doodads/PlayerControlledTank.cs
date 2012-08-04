using System;
using System.Windows.Input;
using FarseerPhysics.Dynamics;
using GalaSoft.MvvmLight.Command;
using Microsoft.Xna.Framework;

namespace BaconGameJam.Common.Models.Doodads
{
    public class PlayerControlledTank : Tank
    {
        private readonly DoodadFactory doodadFactory;

        public PlayerControlledTank(
            DoodadFactory doodadFactory,
            World world, 
            Team team, 
            Vector2 position, 
            float rotation)
            : base(world, team, position, rotation)
        {
            this.doodadFactory = doodadFactory;
            this.FireMissileCommand = new RelayCommand<Vector2>(this.FireMissile);
        }

        public ICommand FireMissileCommand { get; private set; }

        protected override Category CollisionCategory
        {
            get { return Constants.PlayerCategory; }
        }

        private void FireMissile(Vector2 target)
        {
            Vector2 delta = Vector2.Subtract(target, this.Position);
            float theta = (float)Math.Atan2(delta.Y, delta.X);
            this.Heading = theta + MathHelper.PiOver2;

            this.doodadFactory.CreateDoodad(
                new DoodadPlacement()
                    {
                        DoodadType = DoodadType.Missile,
                        Position = this.Position,
                        Rotation = this.Heading - MathHelper.PiOver2,
                        Team = this.Team
                    });
        }
        }
}