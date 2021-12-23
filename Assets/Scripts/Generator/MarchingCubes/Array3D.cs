using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Default {
    public class Array3D<T> {
        #region fields
        public Vector3Int Length { get; private set; }
        public T[] Arr { get; private set; }
        #endregion

        public Array3D(Vector3Int lengthVector) {
            this.Length = lengthVector;
            Arr = new T[lengthVector.x * lengthVector.y * lengthVector.z];
        }

        public Array3D(T[,,] arr3D) {
            Length = new Vector3Int(arr3D.GetLength(0), arr3D.GetLength(1), arr3D.GetLength(2));
            Arr = new T[Length.x * Length.y * Length.z];
            for (int x = 0; x < Length.x; x++) {
                for (int y = 0; y < Length.y; y++) {
                    for (int z = 0; z < Length.z; z++) {
                        this[x, y, z] = arr3D[x, y, z];
                    }
                }
            }
        }

        #region accessors
        public T this[int x, int y, int z] {
            get {
                return Arr[ToIndex(x, y, z)];
            }
            set {
                Arr[ToIndex(x, y, z)] = value;
            }
        }

        private int ToIndex(int x, int y, int z) {
            //return x * Length.y * Length.z + y * Length.z + z;
            return x + (y * Length.x) + (z * Length.x * Length.y);
        }

        public static int ToIndex(int x, int y, int z, Vector3Int Length) {
            return x + (y * Length.x) + (z * Length.x * Length.y);
        }
        #endregion

        public void Clear() {
            for (int i = 0; i < Arr.Length; i++) {
                Arr[i] = default;
            }
        }
    }
}