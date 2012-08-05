using System;
using System.Windows.Input;
using System.Collections.ObjectModel;
using BaconGameJam.Common.Models.Sounds;
using FarseerPhysics.Dynamics;
using GalaSoft.MvvmLight.Command;
using Microsoft.Xna.Framework;

namespace BaconGameJam.Common.Models.Doodads
{
    public class PlayerControlledTank : Tank
    {
        private bool isMoving;
        const float DEFAULT_SPEED = 0.035f;
        float speed;
        bool hasPowerUp;
        DateTime powerUpTime;

        public PlayerControlledTank(
            ISoundManager soundManager,
            DoodadFactory doodadFactory,
            World world,
            Collection<IDoodad> doodads,
            Team team, 
            Vector2 position, 
            float rotation)
            : base(soundManager, world, doodads, team, position, rotation, doodadFactory)
        {
            this.FireMissileCommand = new RelayCommand<Vector2>(this.FireMissile, this.CanFireMissile);
            this.PointTurretCommand = new RelayCommand<Vector2>(this.PointTurret);
            speed = DEFAULT_SPEED;
            hasPowerUp = false;
            powerUpTime = DateTime.Now;
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
            if (hasPowerUp)
            {
                TimeSpan powerUpTS = DateTime.Now - this.powerUpTime;
                if (powerUpTS.TotalSeconds >= 5)
                {
                    hasPowerUp = false;
                    speed = DEFAULT_SPEED;
                }
            }

            if (this.MovingUp || this.MovingDown)
            {
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
                this.TrackMove();
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

        public void GivePowerUp()
        {
            hasPowerUp = true;
            powerUpTime = DateTime.Now;
            speed = 0.07f;
        }
    }
}