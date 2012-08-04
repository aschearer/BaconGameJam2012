using System;
using BaconGameJam.Win7.Views.Tweens.Easings;

namespace BaconGameJam.Win7.Views.Tweens
{
    public static class TweenFactory
    {
        public static ITween Tween(float start, float target, TimeSpan targetRunTime)
        {
            return TweenFactory.Tween(start, target, targetRunTime, EasingFunction.Linear);
        }

        public static ITween Tween(float start, float target, TimeSpan targetRunTime, EasingFunction easingFunction)
        {
            return new Tween(start, target, targetRunTime, TweenFactory.GetEasingFunctionFor(easingFunction));
        }

        private static IEasing GetEasingFunctionFor(EasingFunction easingFunction)
        {
            switch (easingFunction)
            {
                case EasingFunction.Linear:
                    return new LinearEasing();
                case EasingFunction.Quadratic:
                    return new QuadraticEasing();
                default:
                    throw new ArgumentOutOfRangeException("easingFunction");
            }
        }
    }
}