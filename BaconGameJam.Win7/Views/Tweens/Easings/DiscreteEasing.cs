using System;

namespace BaconGameJam.Win7.Views.Tweens.Easings
{
    public class DiscreteEasing : IEasing
    {
        public float Ease(
            float startingValue, 
            float targetValue, 
            TimeSpan targetRunTime, 
            TimeSpan elapsedTime)
        {
            return elapsedTime < targetRunTime ? startingValue : targetValue;
        }
    }
}