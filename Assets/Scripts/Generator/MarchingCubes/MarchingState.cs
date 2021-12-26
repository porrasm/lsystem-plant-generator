using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Default {

    public class MarchingState {
        #region fields
        public float Scale { get; private set; }
        public Array3D<float> Values { get; set; }
        private List<Vector3> vertices;
        private List<int> triangles;

        public int VertexCount => vertices.Count;
        public int TriangleCount => triangles.Count;
        #endregion

        public MarchingState(Vector3Int lengthVector, float scale) {
            this.Scale = scale;
            Init(new Array3D<float>(lengthVector));
        }
        public MarchingState(Array3D<float> values, float scale) {
            this.Scale = scale;
            Init(values);
        }

        private void Init(Array3D<float> values) {
            this.Values = values;
            vertices = new List<Vector3>();
            triangles = new List<int>();
        }

        public Mesh BuildMesh() {
            Mesh mesh = new Mesh();
            mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;

            Debug.Log("Building mesh: " + vertices.Count + " vertices, " + triangles.Count + " triangles");

            mesh.SetVertices(vertices);
            mesh.SetTriangles(triangles, 0);

            mesh.RecalculateBounds();
            mesh.RecalculateNormals();

            return mesh;
        }

        public void AddVertex(Vector3 v) => vertices.Add(v * Scale);
        public void AddTriangle(int v) => triangles.Add(v);
    }
}