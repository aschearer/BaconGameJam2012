using System;
using System.Collections.ObjectModel;
using BaconGameJam.Common.Models.Sounds;
using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Contacts;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;

namespace BaconGameJam.Common.Models.Doodads
{
    public class PowerUp : IDoodad
    {
        private readonly ISoundManager soundManager;
        private readonly World world;
        private readonly Body body;
        private readonly Collection<IDoodad> doodads;
        private DoodadFactory doodadFactory;
        public PowerUpType powerUp;

        private TimeSpan timer;

        public PowerUp(
            ISoundManager soundManager, 
            World world,
            Random random, 
            Collection<IDoodad> doodads, 
            Vector2 position, 
            DoodadFactory doodadFactory)
        {
            this.soundManager = soundManager;
            this.doodadFactory = doodadFactory;
            this.world = world;
            this.doodads = doodads;
            this.body = BodyFactory.CreateBody(world, position, this);
            this.body.BodyType = BodyType.Dynamic;
            this.body.FixedRotation = true;

            PowerUpType powerUpType = PowerUpType.None;
            switch (random.Next(3))
            {
                case 0:
                    powerUpType = PowerUpType.Speed;
                    break;
                case 1:
                    powerUpType = PowerUpType.UnlimitedAmmo;
                    break;
                case 2:
                    powerUpType = PowerUpType.ExtraBounce;
                    break;
            }
            this.powerUp = powerUpType;

            CircleShape shape = new CircleShape(5 / Constants.PixelsPerMeter, 0.1f);
            Fixture fixture = body.CreateFixture(shape);
            fixture.IsSensor = true;
            fixture.CollisionCategories = PhysicsConstants.SensorCategory;
            fixture.CollidesWith = PhysicsConstants.PlayerCategory;
        }

        public Vector2 Position
        {
            get { return this.body.Position; }
        }

        public bool IsDead { get; private set; }

        public Vector2 Velocity
        {
            get { return this.body.LinearVelocity; }
        }

        public void Update(GameTime gameTime)
        {
            ContactEdge edge = this.body.ContactList;
            while (edge != null)
            {
                if (edge.Contact.IsTouching())
                {
                    Fixture f = this.body.Equals(edge.Contact.FixtureA.Body)
                                        ? edge.Contact.FixtureB
                                        : edge.Contact.FixtureA;

                    this.HandleCollision(f);
                    break;
                }

                edge = edge.Next;
            }

            this.timer += gameTime.ElapsedGameTime;
            if (this.timer.TotalSeconds > 15)
            {
                this.RemoveFromGame();
            }

        }

        public void RemoveFromGame()
        {
            this.world.RemoveBody(this.body);
            this.doodads.Remove(this);
        }

        private void HandleCollision(Fixture fixture)
        {
            if (fixture.Body.UserData == null)
            {
                return;
            }

            if (fixture.CollisionCategories == PhysicsConstants.PlayerCategory)
            {
                //this.soundManager.PlaySound("PowerUp");
                PlayerControlledTank tank = (PlayerControlledTank)fixture.Body.UserData;
                tank.GivePowerUp(this.powerUp);
                this.RemoveFromGame();
            }
        }
    }
}