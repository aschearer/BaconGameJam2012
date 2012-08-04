using System;

namespace BaconGameJam.Win7.Views.Tweens.Easings
{
    public interface IEasing
    {
        float Ease(
            float startingValue, 
            float targetValue, 
            TimeSpan targetRunTime, 
            TimeSpan elapsedTime);
    }
}