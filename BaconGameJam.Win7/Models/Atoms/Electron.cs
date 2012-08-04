using System;
using Microsoft.Xna.Framework;

namespace BaconGameJam.Win7.Models.Atoms
{
    public class Electron
    {
        private const float ShellRadius = 32;
        private const float ThetaPerSecond = MathHelper.PiOver4;

        private readonly Atom atom;
        private float theta;

        public Electron(Atom atom, int shell)
        {
            this.atom = atom;
            this.Shell = shell;
            this.Position = Vector2.Add(this.atom.Position, new Vector2(0, -this.Radius));
        }

        public Vector2 Position { get; private set; }
        public int Shell { get; set; }

        private float Radius
        {
            get { return Atom.Radius + (Electron.ShellRadius * this.Shell); }
        }

        public void Update(GameTime gameTime)
        {
            this.theta += (float)gameTime.ElapsedGameTime.TotalSeconds * Electron.ThetaPerSecond;
            Vector2 adjustedPosition = new Vector2((float)Math.Cos(this.theta), (float)Math.Sin(this.theta));
            this.Position = Vector2.Add(this.atom.Position, adjustedPosition * this.Radius);
        }
    }
}