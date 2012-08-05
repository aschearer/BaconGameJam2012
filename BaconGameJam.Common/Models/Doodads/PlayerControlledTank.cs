using System;
using System.Windows.Input;
using System.Collections.ObjectModel;
using FarseerPhysics.Dynamics;
using GalaSoft.MvvmLight.Command;
using Microsoft.Xna.Framework;

namespace BaconGameJam.Common.Models.Doodads
{
    public class PlayerControlledTank : Tank
    {
        private readonly World world;
        private bool isMoving;

        public PlayerControlledTank(
            DoodadFactory doodadFactory,
            World world,
            Collection<IDoodad> doodads,
            Team team, 
            Vector2 position, 
            float rotation)
            : base(world, doodads, team, position, rotation, doodadFactory)
        {
            this.world = world;
            this.FireMissileCommand = new RelayCommand<Vector2>(this.FireMissile, this.CanFireMissile);
            this.PointTurretCommand = new RelayCommand<Vector2>(this.PointTurret);
        }

        public override bool IsMoving
        {
            get { return this.isMoving; }
        }

        public ICommand FireMissileCommand { get; private set; }

        public ICommand PointTurretCommand { get; private set; }

        protected override void OnRemoveFromGame(World world)
        {
        }

        protected override void OnUpdate(GameTime gameTime)
        {
            if (this.MovingUp || this.MovingDown)
            {
                float speed = 0.035f;
                Vector2 direction = new Vector2((float)Math.Cos(this.Rotation + MathHelper.PiOver2), (float)Math.Sin(this.Rotation + MathHelper.PiOver2));
                direction.Normalize();
                direction *= this.MovingUp ? -speed : speed;

                this.Body.Position += direction;
            }

            if (this.MovingLeft || this.MovingRight)
            {
                float torque = MathHelper.PiOver4 / 16;
                this.Body.Rotation += this.MovingLeft ? -torque : torque;
            }

            this.isMoving = this.MovingLeft | this.MovingRight | this.MovingUp | this.MovingDown;
            if (this.isMoving)
            {
                this.Body.Awake = true;
            }
        }

        protected override Category CollisionCategory
        {
            get { return PhysicsConstants.PlayerCategory; }
        }

        private void FireMissile(Vector2 target)
        {
            Vector2 delta = Vector2.Subtract(target, this.Position);
            float theta = (float)Math.Atan2(delta.Y, delta.X);
            this.FireAtTarget(theta);
        }

        private void PointTurret(Vector2 target)
        {
            Vector2 delta = Vector2.Subtract(target, this.Position);
            float theta = (float)Math.Atan2(delta.Y, delta.X);
            this.Heading = theta + MathHelper.PiOver2;
        }
    }
}