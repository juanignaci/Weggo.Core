using System.Collections.Generic;
using UnityEngine;

namespace Weggo.Utils
{
    public static class Helper
    {
        public const float FRAMETIME = 0.01667f;
        //Dunno why i did this, but it's cool i guess?
        public static T WeightedRandom<T>(params KeyValuePair<float, T>[] weightValuePair)
        {
            float f = 0;

            for (int i = 0; i < weightValuePair.Length; i++) { f += weightValuePair[i].Key; }

            var value = Random.value * f;

            for (int i = 0; i < weightValuePair.Length; i++)
            {
                if (value <= weightValuePair[i].Key)
                    return weightValuePair[i].Value;
                else value -= weightValuePair[i].Key;
            }

            return weightValuePair[weightValuePair.Length - 1].Value;
        }

        public static T WeightedRandom<T>(RNG generator, params KeyValuePair<float, T>[] weightValuePair)
        {
            float f = 0;

            for (int i = 0; i < weightValuePair.Length; i++) { f += weightValuePair[i].Key; }

            var value = generator.value * f;

            for (int i = 0; i < weightValuePair.Length; i++)
            {
                if (value <= weightValuePair[i].Key)
                    return weightValuePair[i].Value;
                else value -= weightValuePair[i].Key;
            }

            return weightValuePair[weightValuePair.Length - 1].Value;
        }

        public static float Map(float value, float inMin, float inMax, float outMin, float outMax)
        {
            return ((value - inMin) / (inMax - inMin)) * (outMax - outMin) + outMin;
        }

        public static float Map01(float value, float outMin, float outMax)
        {
            return value * (outMax - outMin) + outMin;
        }

        public static float MapTo01(float value, float inMin, float inMax)
        {
            return Mathf.Clamp01((value - inMin) / (inMax - inMin));
        }

        public static Vector3 V2ToV3(Vector2 bvToolkitVector)
        {
            return new Vector3(bvToolkitVector.x, 0, bvToolkitVector.y);
        }

        public static AnimationCurve AddCurves(params AnimationCurve[] curves)
        {
            var c = new AnimationCurve();

            for (int i = 0; i < curves.Length; i++)
            {
                foreach (var key in curves[i].keys) { c.AddKey(key); }
            }

            return c;
        }

        public static Vector3 XYToXZ(Vector3 v) { return new Vector3(v.x, 0, v.y); }
    }

    public class Timer
    {
        static Dictionary<string, float> timers = new Dictionary<string, float>();

        public static void ClearTimers()
        {
            timers.Clear();
        }

        public static void Set(string name)
        {
            if (!timers.ContainsKey(name))
                timers.Add(name, -1);

            timers[name] = Time.timeSinceLevelLoad;
        }

        public static bool IsSet(string name) { return timers.ContainsKey(name) && timers[name] != -1; }

        public static void Reset(string name)
        {
            if (!timers.ContainsKey(name))
                Debug.LogError(string.Format("Timer {0} is not set!", name));
            else timers[name] = -1;
        }

        public static void Remove(string name)
        {
            if (!timers.ContainsKey(name))
                Debug.LogError(string.Format("Timer {0} is not set!", name));
            else timers.Remove(name);
        }

        public static bool ReachedTime(string name, float target, bool autoReset = true)
        {
            if (!IsSet(name))
            {
                Debug.LogError(string.Format("Timer {0} is not set!", name));
                return false;
            }
            else if (Time.timeSinceLevelLoad - timers[name] >= target)
            {
                if (autoReset) Reset(name);
                return true;
            }
            else return false;
        }

        public static bool ReachedTime(string name, float target, out float progress, bool autoReset = true)
        {
            if (!IsSet(name))
            {
                Debug.LogError(string.Format("Timer {0} is not set!", name));
                progress = -1;

                return false;
            }
            else
            {
                var pp = Mathf.Clamp01((Time.timeSinceLevelLoad - timers[name]) / target);
                progress = pp;

                if (pp == 1) { if (autoReset) timers[name] = -1; return true; }
                else return false;
            }
        }
    }

    public class RNG
    {
        const int N = int.MaxValue, A = 48271;
        public readonly int seed;
        uint lValue;

        public float value { get { return Next(); } }

        public RNG(int seed = -1)
        {
            if (seed < 0) { seed = new System.Random().Next(N); }

            this.seed = seed % N;
            lValue = (uint)this.seed;
        }

        public int NextInt() { lValue = A * lValue % N; return (int)lValue; }
        public float Next() { lValue = A * lValue % N; return (float)lValue / N; }
        public float Range(float min, float max) { return (max - min) * value + min; }
        public int Range(int min, int max) { return (int)Range((float)min, max); }

        public float Perlin(float x, float y) { return Mathf.PerlinNoise(seed / N + x, seed / N + y); }

        public T WeightedRandom<T>(params KeyValuePair<float, T>[] weightValuePair)
        {
            float f = 0;

            for (int i = 0; i < weightValuePair.Length; i++) { f += weightValuePair[i].Key; }

            var val = value * f;

            for (int i = 0; i < weightValuePair.Length; i++)
            {
                if (val <= weightValuePair[i].Key)
                    return weightValuePair[i].Value;
                else val -= weightValuePair[i].Key;
            }

            return weightValuePair[weightValuePair.Length - 1].Value;
        }


        public T WeightedRandom<T>(T[] possibilities, float[] weights)
        {
            if(weights.Length != possibilities.Length)
            {
                Debug.LogError("Possibilities and weights should have the same Lenght!");
                return possibilities[0];
            }

            float f = 0;

            for (int i = 0; i < weights.Length; i++) { f += weights[i]; }

            var val = value * f;

            for (int i = 0; i < possibilities.Length; i++)
            {
                if (val <= weights[i])
                    return possibilities[i];
                else val -= weights[i];
            }

            return possibilities[possibilities.Length - 1];
        }

        public int WeightedRandom(float[] weights)
        {
            float f = 0;

            for (int i = 0; i < weights.Length; i++) { f += weights[i]; }

            var val = value * f;

            for (int i = 0; i < weights.Length; i++)
            {
                if (val <= weights[i])
                    return i;
                else val -= weights[i];
            }

            return weights.Length-1;
        }
    }
}