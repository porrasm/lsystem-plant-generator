using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Default {
    public static class Matht {

        public static float Percentage(float min, float max, float x) {
            return (x - min) / (max - min);
        }
        public static float ToRange(float prevLow, float prevHigh, float newLow, float newHigh, float value) {
            return newLow + (newHigh - newLow) * Percentage(prevLow, prevHigh, value);
        }

        public static float BCubic(float c00, float c10, float c01, float c11, float tx, float ty) {
            return Cubic(Cubic(c00, c10, tx), Cubic(c01, c11, tx), ty);
        }
        public static float Cubic(float a, float b, float t) {
            float newT = -2 * Mathf.Pow(t, 3) + 3 * Mathf.Pow(t, 2);
            return Lerp(a, b, newT);
        }

        public static float Lerp(float a, float b, float t) {
            return a * (1 - t) + b * t;
        }
        public static float Blerp(float c00, float c10, float c01, float c11, float tx, float ty) {
            return Lerp(Lerp(c00, c10, tx), Lerp(c01, c11, tx), ty);
        }

        public static float HexagonicLerp(float a, float b, float t) {
            return Lerp(a, b, 6 * Mathf.Pow(t, 5) - 15 * Mathf.Pow(t, 4) + 10 * Mathf.Pow(t, 3));
        }

        public static int Clamp(int val, int min = 0, int max = 0) {
            if (max <= min) {
                return val;
            } else if (val > max) {
                return max;
            } else if (val < min) {
                return min;
            } else {
                return val;
            }
        }
        public static float Clamp(float val, float min = 0, float max = 0) {
            if (max <= min) {
                return val;
            } else if (val > max) {
                return max;
            } else if (val < min) {
                return min;
            } else {
                return val;
            }
        }

        public static Color Interpolate(Color start, Color end, float t, Func<float, float> curve) {
            float newT = curve(t);
            float r = Lerp(start.r, end.r, newT);
            float g = Lerp(start.g, end.g, newT);
            float b = Lerp(start.b, end.b, newT);
            float a = Lerp(start.a, end.a, newT);
            return new Color(r, g, b, a);
        }
    }
}