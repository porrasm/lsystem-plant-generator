using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Default {
    public class ExtendableArray3D<T> {
        #region fields
        private Dictionary<Vector3Int, T> values;
        private Func<T> defaultValue;

        private Vector2Int xRange;
        private Vector2Int yRange;
        private Vector2Int zRange;

        public int CountX => xRange.y - xRange.x;
        public int CountY => yRange.y - yRange.x;
        public int CountZ => zRange.y - zRange.x;
        #endregion

        public ExtendableArray3D(Func<T> defaultValue) {
            this.defaultValue = defaultValue;
            values = new Dictionary<Vector3Int, T>();
            this[Vector3Int.zero] = defaultValue();
        }

        public T this[int x, int y, int z] {
            get {
                return this[new Vector3Int(x, y, z)];
            }
            set {
                this[new Vector3Int(x, y, z)] = value;
            }
        }

        public T this[Vector3Int v] {
            get {
                if (values.TryGetValue(v, out T value)) {
                    return value;
                }
                return defaultValue();
            }
            set {
                if (values.ContainsKey(v)) {
                    values[v] = value;
                } else {
                    UpdateBounds(v);
                    values.Add(v, value);
                }
            }
        }

        public void ExtendRanges() {
            xRange.x--;
            xRange.y++;
            yRange.x--;
            yRange.y++;
            zRange.x--;
            zRange.y++;
        }

        private void UpdateBounds(Vector3Int index) {
            xRange.x = Math.Min(xRange.x, index.x);
            xRange.y = Math.Max(xRange.y, index.x);

            yRange.x = Math.Min(yRange.x, index.y);
            yRange.y = Math.Max(yRange.y, index.y);

            zRange.x = Math.Min(zRange.x, index.z);
            zRange.y = Math.Max(zRange.y, index.z);
        }

        public B[,,] To3DArray<B>(out Vector3Int origo, Func<T, B> transformation) {
            Logger.LogVariables("xr", xRange);
            Logger.LogVariables("yr", yRange);
            Logger.LogVariables("zr", zRange);

            B[,,] result = new B[CountX + 1, CountY + 1, CountZ + 1];

            origo = new Vector3Int(-xRange.x, -yRange.x, -zRange.x);

            for (int x = xRange.x; x <= xRange.y; x++) {
                int endX = x - xRange.x;

                for (int y = yRange.x; y <= yRange.y; y++) {
                    int endY = y - yRange.x;

                    for (int z = zRange.x; z <= zRange.y; z++) {
                        int endZ = z - zRange.x;
                        result[endX, endY, endZ] = transformation(this[new Vector3Int(x, y, z)]);
                    }
                }
            }

            return result;
        }
    }
}
