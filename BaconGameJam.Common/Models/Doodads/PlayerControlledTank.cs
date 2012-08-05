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
        private readonly DoodadFactory doodadFactory;
        private readonly World world;
        private bool isMoving;

        public PlayerControlledTank(
            DoodadFactory doodadFactory,
            World world,
            Collection<IDoodad> doodads,
            Team team, 
            Vector2 position, 
            float rotation)
            : base(world, doodads, team, position, rotation)
        {
            this.doodadFactory = doodadFactory;
            this.world = world;
            this.FireMissileCommand = new RelayCommand<Vector2>(this.FireMissile, this.CanFireMissile);
        }

        public override bool IsMoving
        {
            get { return this.isMoving; }
        }

        public ICommand FireMissileCommand { get; private set; }

        protected override void OnUpdate(GameTime gameTime)
        {
            return;
            // up/down raycast
            Vector2 rayStart = new Vector2(Position.X, Position.Y);
            Vector2 rayEnd = rayStart + new Vector2(0, (MovingUp ? -1 : (MovingDown ? 1 : 0)));

            this.world.RayCast((fixture, point, normal, fraction) =>
            {
                if ((fixture != null) & (fixture.CollisionCategories == PhysicsConstants.ObstacleCategory))
                {
                    if (MovingUp) MovingUp = false;
                    else if (MovingDown) MovingDown = false;
                    return 1;
                }
                return fraction;
            }, rayStart, rayEnd);

            // left/right raycast
            rayStart = new Vector2(Position.X, Position.Y);
            rayEnd = rayStart + new Vector2((MovingLeft ? -1 : (MovingRight ? 1 : 0)), 0);

            this.world.RayCast((fixture, point, normal, fraction) =>
            {
                if ((fixture != null) & (fixture.CollisionCategories == PhysicsConstants.ObstacleCategory))
                {
                    if (MovingLeft) MovingLeft = false;
                    else if (MovingRight) MovingRight = false;
                    return 1;
                }
                return fraction;
            }, rayStart, rayEnd);

            this.Body.SetTransform(new Vector2(this.Body.Position.X + (MovingLeft ? -0.05f : (MovingRight ? 0.05f : 0)), this.Body.Position.Y + (MovingUp ? -0.05f : (MovingDown ? 0.05f : 0))), this.Heading);
        }

        protected override Category CollisionCategory
        {
            get { return PhysicsConstants.PlayerCategory; }
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

        private bool CanFireMissile(Vector2 target)
        {
            return !this.ContainsPoint(target);
        }
    }
}