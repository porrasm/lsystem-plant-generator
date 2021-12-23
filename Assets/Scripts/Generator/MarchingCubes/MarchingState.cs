using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Default {

    public class MarchingState {

        #region fields
        public Array3D<float> Values { get; set; }
        public List<Vector3> Vertices;
        public List<int> Triangles;
        #endregion

        public MarchingState(Vector3Int lengthVector) {
            Init(new Array3D<float>(lengthVector));
        }
        public MarchingState(Array3D<float> values) {
            Init(values);
        }
        private void Init(Array3D<float> values) {
            this.Values = values;
            Vertices = new List<Vector3>();
            Triangles = new List<int>();
        }

        public Mesh BuildMesh() {
            Mesh mesh = new Mesh();
            mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;

            Debug.Log("Building mesh: " + Vertices.Count + " vertices, " + Triangles.Count + " triangles");

            mesh.SetVertices(Vertices);
            mesh.SetTriangles(Triangles, 0);

            mesh.RecalculateBounds();
            mesh.RecalculateNormals();

            return mesh;
        }
    }
}