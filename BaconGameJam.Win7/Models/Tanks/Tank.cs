using System;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;

namespace BaconGameJam.Win7.Models.Tanks
{
    public class Tank
    {
        private readonly Body body;

        public Tank(World world, Team team, Vector2 position, float rotation)
        {
            this.body = BodyFactory.CreateBody(world, position, this);
            this.body.Rotation = rotation;
            this.Team = team;
            this.Heading = rotation;
            this.IsMoving = true;
        }

        public bool IsMoving { get; private set; }

        public Vector2 Position
        {
            get { return this.body.Position; }
        }

        public float Rotation
        {
            get { return this.body.Rotation; }
        }

        public Team Team { get; set; }
        public float Heading { get; private set; }

        public void Update(GameTime gameTime)
        {
        }
    }
}