using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

        // Still not as fast as possible, doesnt efficiently find starting index
        public static List<Vector3Int> GetAllPointsInCylinder(Cylinder c) {
            List<Vector3Int> points = new List<Vector3Int>();

            int minX = Mathf.FloorToInt(-c.Radius + Mathf.Min(c.P1.x, c.P2.x));
            int maxX = Mathf.CeilToInt(c.Radius + Mathf.Max(c.P1.x, c.P2.x));

            int minY = Mathf.FloorToInt(-c.Radius + Mathf.Min(c.P1.y, c.P2.y));
            int maxY = Mathf.CeilToInt(c.Radius + Mathf.Max(c.P1.y, c.P2.y));

            int minZ = Mathf.FloorToInt(-c.Radius + Mathf.Min(c.P1.z, c.P2.z));
            int maxZ = Mathf.CeilToInt(c.Radius + Mathf.Max(c.P1.z, c.P2.z));

            for (int x = minX; x <= maxX; x++) {
                Range foundY = new Range(1, 0);

                for (int y = minY; y <= maxY; y++) {
                    Range foundZ = new Range(1, 0);

                    for (int z = minZ; z <= maxZ; z++) {
                        Vector3Int p = new Vector3Int(x, y, z);
                        if (c.Contains(p)) {
                            points.Add(p);
                            foundY.Extend(y);
                            foundZ.Extend(z);
                        }
                    }

                    //if (foundZ.IsValid()) {
                    //    minZ = foundZ.Min - 1;
                    //    maxZ = foundZ.Max + 1;
                    //}
                }

                //if (foundY.IsValid()) {
                //    minY = foundY.Min - 1;
                //    maxY = foundY.Max + 1;
                //}
            }

            return points;
        }

        public static Vector3 Multiply(Vector3 a, Vector3 b) => new Vector3(a.x * b.x, a.y * b.y, a.z * b.z);
        public static Vector3 Divide(Vector3 a, Vector3 b) => new Vector3(a.x / b.x, a.y / b.y, a.z / b.z);
        public static Vector3 Modulo(Vector3 a, Vector3 b) => new Vector3(a.x % b.x, a.y % b.y, a.z % b.z);
        public static Vector3 Max(Vector3 v, Vector3 other) {
            return new Vector3(Mathf.Max(v.x, other.x), Mathf.Max(v.y, other.y), Mathf.Max(v.z, other.z));
        }
        public static Vector3 Min(Vector3 v, Vector3 other) {
            return new Vector3(Mathf.Min(v.x, other.x), Mathf.Min(v.y, other.y), Mathf.Min(v.z, other.z));
        }
    }
}