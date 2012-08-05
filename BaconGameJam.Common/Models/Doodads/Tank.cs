using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;
using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;

namespace BaconGameJam.Common.Models.Doodads
{
    public abstract class Tank : IDoodad
    {
        private readonly World world;
        private readonly Body body;
        private Collection<IDoodad> doodads;
        private DoodadFactory doodadFactory;
        private List<Missile> activeMissiles;
        private TimeSpan elapsedTime;

        protected Tank(
            World world, 
            Collection<IDoodad> doodads, 
            Team team, 
            Vector2 position, 
            float rotation, 
            DoodadFactory doodadFactory)
        {
            this.world = world;
            this.doodadFactory = doodadFactory;
            this.doodads = doodads;
            this.body = BodyFactory.CreateBody(world, position, this);
            this.body.Rotation = rotation;
            this.body.BodyType = BodyType.Dynamic;
            this.Team = team;
            this.Heading = rotation;
            this.activeMissiles = new List<Missile>();

            var shape = new PolygonShape(0);
            shape.SetAsBox(15 / Constants.PixelsPerMeter, 15 / Constants.PixelsPerMeter);
            var fixture = this.body.CreateFixture(shape);
            fixture.CollisionCategories = this.CollisionCategory;
            fixture.CollidesWith = PhysicsConstants.EnemyCategory | PhysicsConstants.ObstacleCategory |
                                   PhysicsConstants.MissileCategory | PhysicsConstants.PitCategory;
        }

        public abstract bool IsMoving { get; }

        public Vector2 Position
        {
            get { return this.body.Position; }
        }

        public float Rotation
        {
            get { return this.body.Rotation; }
        }

        public Team Team { get; set; }

        public float Heading { get; set; }

        public bool MovingUp { get; set; }
        public bool MovingDown { get; set; }
        public bool MovingLeft { get; set; }
        public bool MovingRight { get; set; }

        protected Body Body
        {
            get { return this.body; }
        }

        protected virtual Category CollisionCategory
        {
            get { return PhysicsConstants.EnemyCategory; }
        }

        public void Update(GameTime gameTime)
        {
            this.elapsedTime += gameTime.ElapsedGameTime;
            this.OnUpdate(gameTime);
            for (int i = this.activeMissiles.Count - 1; i >= 0; i--)
            {
                if (this.activeMissiles[i].IsDead)
                {
                    this.activeMissiles.RemoveAt(i);
                }
            }
        }

        public void RemoveFromGame()
        {
            this.world.RemoveBody(this.body);
            this.doodads.Remove(this);
            this.OnRemoveFromGame(this.world);
            this.activeMissiles.Clear();
        }

        public bool CanFireMissile(Vector2 target)
        {
            return this.activeMissiles.Count < Constants.MaxNumberOfMissiles && 
                this.elapsedTime.TotalSeconds > 0.3 &&
                !this.ContainsPoint(target);
        }

        public void FireAtTarget(float theta)
        {
            this.elapsedTime = TimeSpan.Zero;
            this.Heading = theta + MathHelper.PiOver2;
            Vector2 distance = new Vector2((float)Math.Cos(theta), (float)Math.Sin(theta));
            distance *= 0.9f;

            this.activeMissiles.Add(
                (Missile)this.doodadFactory.CreateDoodad(
                    new DoodadPlacement()
                        {
                            DoodadType = DoodadType.Missile,
                            Position = this.Position + distance,
                            Rotation = this.Heading - MathHelper.PiOver2,
                            Team = this.Team
                        }));
        }

        protected abstract void OnRemoveFromGame(World world);

        protected bool ContainsPoint(Vector2 point)
        {
            bool b = this.world.TestPointAll(point).Any(fixture => this.Equals(fixture.Body.UserData));
            return b;
        }

        protected virtual void OnUpdate(GameTime gameTime)
        {
        }
    }
}