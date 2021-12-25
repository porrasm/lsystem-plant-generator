using UnityEngine;

namespace Default {
    public static class Vector3Extension {
        public static Vector3 Rotate(this Vector3 v, float angle, Vector3 axis) {
            return Quaternion.AngleAxis(angle, axis) * v;
        }

        #region multiplication
        public static Vector3 Multiply(this Vector3 v, Vector3 other) {
            return new Vector3(v.x * other.x, v.y * other.y, v.z * other.z);
        }

        public static Vector3 Multiply(this Vector3 v, float x, float y, float z) {
            return new Vector3(v.x * x, v.y * y, v.z * z);
        }

        public static Vector3 Multiply(this Vector3 v, float amount) {
            return new Vector3(v.x * amount, v.y * amount, v.z * amount);
        }
        #endregion

        #region division
        public static Vector3 Divide(this Vector3 v, Vector3 other) {
            return new Vector3(v.x / other.x, v.y / other.y, v.z / other.z);
        }

        public static Vector3 Divide(this Vector3 v, float x, float y, float z) {
            return new Vector3(v.x / x, v.y / y, v.z / z);
        }

        public static Vector3 Divide(this Vector3 v, float amount) {
            return new Vector3(v.x / amount, v.y / amount, v.z / amount);
        }
        #endregion

        public static Vector3 Add(this Vector3 v, float x, float y, float z) {
            return new Vector3(v.x + x, v.y + y, v.z + z);
        }

        public static Vector3 SetX(this Vector3 v, float x) {
            return new Vector3(x, v.y, v.z);
        }

        public static Vector3 SetY(this Vector3 v, float y) {
            return new Vector3(v.x, y, v.z);
        }

        public static Vector3 SetZ(this Vector3 v, float z) {
            return new Vector3(v.x, v.y, z);
        }
    }
}
