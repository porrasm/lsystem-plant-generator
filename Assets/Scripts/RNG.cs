using System;
using UnityEngine;

namespace Default {
    public static class RNG {

        private static Xorshift128 rnd;

        static RNG() {
            System.Random poorRnd = new System.Random();
            rnd = new Xorshift128(poorRnd.Next(), poorRnd.Next(), poorRnd.Next(), poorRnd.Next());
        }

        public static T Seeded<T>(bool useSeed, int seed, Func<T> cb) {
            if (useSeed) {
                return Seeded(seed, cb);
            } else {
                return cb();
            }
        }

        public static void Seeded(bool useSeed, int seed, Action cb) {
            if (useSeed) {
                Seeded(seed, cb);
            } else {
                cb?.Invoke();
            }
        }

        public static void Seeded(int seed, Action cb) {
            UnityEngine.Random.State old = UnityEngine.Random.state;
            var old2 = rnd.GetState();

            rnd.SetSeed(seed);
            UnityEngine.Random.InitState(seed);
            cb?.Invoke();

            Logger.Log("Reset seed");
            UnityEngine.Random.state = old;
            rnd.SetState(old2);
        }

        public static T Seeded<T>(int seed, Func<T> cb) {
            UnityEngine.Random.State old = UnityEngine.Random.state;
            var old2 = rnd.GetState();

            rnd.SetSeed(seed);
            UnityEngine.Random.InitState(seed);
            T val = cb();

            Logger.Log("Reset seed");
            UnityEngine.Random.state = old;
            rnd.SetState(old2);

            return val;
        }

        //public static void SetSeed(int seed) {
        //    UnityEngine.Random.InitState(seed);
        //    rnd.SetSeed(seed);
        //}

        public static float Float {
            get {
                return UnityEngine.Random.value;
            }
        }

        public static float Range(float min, float max) {
            return UnityEngine.Random.Range(min, max);
        }

        public static int Range(int min, int max, bool inclusive) {
            return UnityEngine.Random.Range(min, max + (inclusive ? 1 : 0));
        }

        public static int Int {
            get {
                return rnd.Next;
            }
        }

        public static Vector3 Vector {
            get {
                return new Vector3(Float, Float, Float);
            }
        }
        public static Vector3 Direction {
            get {
                return new Vector3(Float, Float, Float).normalized;
            }
        }

        public static float TransformedFloat(float f) {
            throw new NotImplementedException();
        }

        public static int[] SeededPermutationTable(int size = 256) {

            int[] ordered = new int[size];
            for (int i = 0; i < size; i++) {
                ordered[i] = i;
            }

            int count = size;

            int[] table = new int[size];

            for (int i = 0; i < size; i++) {
                int ri = UnityEngine.Random.Range(0, count);
                table[i] = ordered[ri];
                count--;
                ordered[ri] = ordered[count];
            }

            return table;
        }
    }
}
