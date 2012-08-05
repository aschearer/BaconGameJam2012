using System;
using Microsoft.Xna.Framework;

namespace BaconGameJam.Win7.ViewModels.Transitions
{
    public class SimpleTransitionViewModel : ITransition
    {
        public event EventHandler<EventArgs> Closed;

        private readonly TimeSpan runtime;
        private TimeSpan elapsedTime;

        public SimpleTransitionViewModel(TimeSpan runtime)
        {
            this.runtime = runtime;
        }

        public float PercentClosed
        {
            get { return (float)(this.elapsedTime.TotalSeconds / this.runtime.TotalSeconds); }
        }

        public void Update(GameTime gameTime)
        {
            if (this.elapsedTime < this.runtime)
            {
                this.elapsedTime += gameTime.ElapsedGameTime;
                if (this.elapsedTime >= this.runtime)
                {
                    this.elapsedTime = this.runtime;
                    if (this.Closed != null)
                    {
                        this.Closed(this, new EventArgs());
                    }
                }
            }
        }
    }
}