using System;
using BaconGameJam.Win7.Views.Tweens.Easings;
using Microsoft.Xna.Framework;

namespace BaconGameJam.Win7.Views.Tweens
{
    public class Tween : ITween
    {
        private float start;
        private float target;
        private readonly TimeSpan targetRunTime;
        private readonly IEasing easing;
        private TimeSpan elapsedTime;
        private int currentIteration;

        public Tween(float start, float target, TimeSpan targetRunTime, IEasing easing)
        {
            this.start = this.Value = start;
            this.target = target;
            this.targetRunTime = targetRunTime;
            this.easing = easing;
            this.elapsedTime = TimeSpan.Zero;
        }

        public float Value { get; private set; }

        public Repeat Repeats { get; set; }

        public bool YoYos { get; set; }

        public bool IsPaused { get; set; }

        public void Update(GameTime gameTime)
        {
            if (this.IsPaused)
            {
                return;
            }

            this.elapsedTime = this.elapsedTime.Add(gameTime.ElapsedGameTime);
            if (this.elapsedTime.CompareTo(this.targetRunTime) < 0)
            {
                this.Value = this.easing.Ease(this.start, this.target, this.targetRunTime, this.elapsedTime);
            }
            else
            {
                bool shouldRepeat = this.Repeats == Repeat.Forever || (int)this.Repeats > this.currentIteration;
                if (shouldRepeat)
                {
                    this.currentIteration++;
                    if (this.YoYos)
                    {
                        this.Reverse();
                    }
                    else
                    {
                        this.elapsedTime = this.elapsedTime.Subtract(this.targetRunTime);
                        this.Value = this.easing.Ease(this.start, this.target, this.targetRunTime, this.elapsedTime);
                    }
                }
                else
                {
                    this.Value = this.target;
                }
            }
        }

        /// <summary>
        /// Flips start and end values and adjusts elapsed time to reflect changes.
        /// </summary>
        public void Reverse()
        {
            float originalStart = this.start;
            this.start = this.target;
            this.target = originalStart;
            this.elapsedTime = this.targetRunTime.Subtract(this.elapsedTime);
        }
    }
}