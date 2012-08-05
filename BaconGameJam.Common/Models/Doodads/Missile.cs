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
    public class Missile : IDoodad
    {
        private readonly ISoundManager soundManager;
        private readonly World world;
        private readonly Body body;
        private readonly Collection<IDoodad> doodads;
        private int obstacleCollisionCtr;
        private TimeSpan elapsedTime;
        private DoodadFactory doodadFactory;

        public Missile(
            ISoundManager soundManager, 
            World world, 
            Collection<IDoodad> doodads, 
            Team team, 
            Vector2 position, 
            float rotation, 
            DoodadFactory doodadFactory)
        {
            this.soundManager = soundManager;
            this.doodadFactory = doodadFactory;
            this.world = world;
            this.doodads = doodads;
            this.body = BodyFactory.CreateBody(world, position, this);
            this.body.BodyType = BodyType.Dynamic;
            this.body.FixedRotation = true;

            CircleShape shape = new CircleShape(5 / Constants.PixelsPerMeter, 0.1f);
            Fixture fixture = body.CreateFixture(shape);
            fixture.Restitution = 1;
            fixture.Friction = 0;
            fixture.CollisionCategories = PhysicsConstants.MissileCategory;
            fixture.CollidesWith = PhysicsConstants.EnemyCategory | PhysicsConstants.PlayerCategory |
                                   PhysicsConstants.ObstacleCategory | PhysicsConstants.MissileCategory |
                                   PhysicsConstants.SensorCategory;
            obstacleCollisionCtr = 0;

            Vector2 force = new Vector2((float)Math.Cos(rotation), (float)Math.Sin(rotation)) * 3;
            this.body.ApplyForce(force);
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
            this.elapsedTime += gameTime.ElapsedGameTime;

            ContactEdge edge = this.body.ContactList;
            while (edge != null)
            {
                if (edge.Contact.IsTouching())
                {
                    Fixture enemy = this.body.Equals(edge.Contact.FixtureA.Body)
                                        ? edge.Contact.FixtureB
                                        : edge.Contact.FixtureA;

                    this.HandleCollision(enemy);
                    break;
                }

                edge = edge.Next;
            }
        }

        public void RemoveFromGame()
        {
            this.IsDead = true;
            this.world.RemoveBody(this.body);
            this.doodads.Remove(this);
        }

        private void HandleCollision(Fixture fixture)
        {
            if (this.elapsedTime.TotalSeconds < 0.2)
            {
                if (fixture.Body.UserData != null)
                {
                    this.RemoveFromGame();
                }

                return;
            }

            if (fixture.Body.UserData == null)
            {
                return;
            }

            switch (fixture.CollisionCategories)
            {
                case PhysicsConstants.ObstacleCategory:
                    obstacleCollisionCtr++;
                    this.soundManager.PlaySound("MissileBounce");
                    if (obstacleCollisionCtr >= 2)
                    {
                        RemoveFromGame();
                        this.LeaveBlastMark();
                    }
                    break;
                case PhysicsConstants.EnemyCategory:
                case PhysicsConstants.PlayerCategory:
                    this.LeaveBlastMark();
                    RemoveFromGame();
                    this.soundManager.PlaySound("MissileBounce");
                    Tank tank = (Tank)fixture.Body.UserData;
                    tank.Destroy();

                    (fixture.Body.UserData as Tank).RemoveFromGame();
                    break;
                case PhysicsConstants.MissileCategory:
                    this.soundManager.PlaySound("MissileBounce");
                    this.RemoveFromGame();
                    this.LeaveBlastMark();
                    break;
            }
        }

        private void LeaveBlastMark()
        {
            var mark = (BlastMark)this.doodadFactory.CreateDoodad(new DoodadPlacement() { DoodadType = DoodadType.BlastMark, Position = this.Position });
            mark.IsSmall = true;
        }
    }
}