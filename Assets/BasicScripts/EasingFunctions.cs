using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//A static class which provides different easing functions
public static class EasingFunctions
{
    private const float tau = 2*Mathf.PI;
    public enum EasingFunction {None, SmoothStart, SmoothStop, SmootherStep, EaseInOut, Arch, BellCurve, Spike, BounceOut, BounceIn, BounceInOut, ElasticIn, ElasticOut}

    public static float DoEase(EasingFunction type, float t)
    {
        switch(type)
        {
            case EasingFunction.None: return t;
            case EasingFunction.SmoothStart: return SmoothStart(t);
            case EasingFunction.SmoothStop: return SmoothStop(t);
            case EasingFunction.SmootherStep: return SmootherStep(t);
            case EasingFunction.EaseInOut: return EaseInOut(t);
            case EasingFunction.Arch: return Arch(t);
            case EasingFunction.BellCurve: return BellCurve(t);
            case EasingFunction.Spike: return Spike(t);
            case EasingFunction.BounceOut: return BounceOut(t);
            case EasingFunction.BounceIn: return BounceIn(t);
            case EasingFunction.BounceInOut: return BounceInOut(t);
            case EasingFunction.ElasticIn: return ElasticIn(t);
            case EasingFunction.ElasticOut: return ElasticOut(t);
            default: return 0f;
        }
    }

    public static float SmoothStart(float t)
    {
        return t*t;
    }

    public static float SmoothStartX(float t, int order)
    {
        for(int i = 0; i < order - 1; i++)
        {
            t *= t;
        }
        return t;
    }

    public static float SmoothStop(float t)
    {
        return 1 - (1-t)*(1-t);
    }

    public static float SmoothStopX(float t, int order)
    {
        return Mathf.Pow((1 - (1-t)), order - 1);
    }

    public static float SmoothStep(float t)
    {
         return t * t * (3f - 2f * t);
    }

    public static float SmootherStep(float t)
    {
        return t*t*t*(t*(t * 6f - 15f) + 10f);
    }

    public static float EaseInOut(float t)
    {
        return Mathf.Lerp(SmoothStart(t), SmoothStop(t), t);
    }

    public static float Arch(float t)
    {
        return t*(1-t);
    }

    public static float SmoothStartArch(float t)
    {
        return t*t* (1-t);
    }

    public static float SmoothStopArch(float t)
    {
        return t*(1-t)*(1-t);
    }

    public static float BellCurve(float t)
    {
        return SmoothStart(t)*SmoothStop(t);
    }

    //a mirrored function that goes 0->1 then 1->0
    public static float Spike(float t)
    {
        return (t <= 0.5f)? SmoothStart(t / 0.5f) : SmoothStart((1-t)/0.5f);
    }

    //cubic Bezier through ABCD where A (start) and D(end) are 0 and 1
    public static float NormalisedBezier(float b, float c, float t)
    {
        float s = 1f - t;
        float t2 = t*t;
        float s2 = s*s;
        float t3 = t2*t;
        return (3f*b*s2*t) + (3f*c*s*t2) + t3;
    }

    //returns Catmull-Rom spline going through points p0 to p3
    public static float Catmullrom(float t, float p0, float p1, float p2, float p3)
    {
        return 0.5f * (
              (2f * p1) +
              (-p0 + p2) * t +
              (2f * p0 - 5f * p1 + 4f * p2 - p3) * t * t +
              (-p0 + 3f * p1 - 3 * p2 + p3) * t * t * t
              );
    }

    public static float OverShoot(float t, float freq, float decay)
    {
        return Mathf.Sin(t*freq*Mathf.PI*2)/Mathf.Exp(t*decay);
    }

    public static float QuickBounce(float t, float freq, float decay)
    {
        float s = OverShoot(t, freq, decay);
        return (s > 1)? 1-s : (s < 1)? -s : s;    
    }
    
    public static float BounceOut(float t)
    {
        const float n1 = 7.5625f;
        const float d1 = 2.75f;

        if (t < 1 / d1) {
            return n1 * t * t;
        } else if (t < 2 / d1) {
            return n1 * (t -= 1.5f / d1) * t + 0.75f;
        } else if (t < 2.5 / d1) {
            return n1 * (t -= 2.25f / d1) * t + 0.9375f;
        } else {
            return n1 * (t -= 2.625f / d1) * t + 0.984375f;
        }
    }

    public static float BounceIn(float t)
    {
        return 1-BounceOut(1-t);
    }

    public static float BounceInOut(float t)
    {
        return (t<0.5f) ? 0.5f * BounceIn(2f*t) : 0.5f * BounceOut(2f*t-1f);
    }

    //oscillates as sin wave with frequency being cycles per 0->1
    public static float Oscillate(float t, int frequency)
    {
        return Mathf.Sin(t*tau*frequency);
    }

    public static float ElasticIn(float t)
    {
        return t==1 ? 1 : t==0? 0 : -Mathf.Pow(2f, 10f * t - 10f) * Mathf.Sin((t * 10f - 10.75f) * tau/3f);
    }

    public static float ElasticOut(float t)
    {
        return t==1 ? 1 : t==0? 0 : Mathf.Pow(2f, -10f * t) * Mathf.Sin((t * 10f - 0.75f) * tau/3f) + 1;
    }

    
}
