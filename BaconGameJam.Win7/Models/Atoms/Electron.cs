using Microsoft.Xna.Framework;

namespace BaconGameJam.Win7.Models.Atoms
{
    public class Electron
    {
        private const float ShellRadius = 32;

        private readonly Atom atom;

        public Electron(Atom atom, int shell)
        {
            this.atom = atom;
            this.Shell = shell;
            this.Position = Vector2.Add(this.atom.Position, new Vector2(0, -(Atom.Radius + Electron.ShellRadius * this.Shell)));
        }

        public Vector2 Position { get; private set; }
        public int Shell { get; set; }
    }
}