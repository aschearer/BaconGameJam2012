using System;

namespace BaconGameJam.Win7.Views.Tweens.Easings
{
    public class LinearEasing : IEasing
    {
        public float Ease(
            float startingValue, 
            float targetValue, 
            TimeSpan targetRunTime, 
            TimeSpan elapsedTime)
        {
            float delta = targetValue - startingValue;
            return (float)(delta * elapsedTime.TotalSeconds / targetRunTime.TotalSeconds + startingValue);
        }
    }
}