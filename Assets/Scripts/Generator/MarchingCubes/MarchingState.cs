using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Default {

    public class MarchingState {
        #region fields
        public float Scale { get; private set; }
        public Array3D<DensityPoint> Values { get; set; }
        private List<Vector3> vertices;
        private List<int> triangles;
        private List<Color> colors;

        public int VertexCount => vertices.Count;
        public int TriangleCount => triangles.Count;
        #endregion

        public MarchingState(Vector3Int lengthVector, float scale) {
            this.Scale = scale;
            Init(new Array3D<DensityPoint>(lengthVector));
        }
        public MarchingState(Array3D<DensityPoint> values, float scale) {
            this.Scale = scale;
            Init(values);
        }

        private void Init(Array3D<DensityPoint> values) {
            this.Values = values;
            vertices = new List<Vector3>();
            triangles = new List<int>();
            colors = new List<Color>();
        }

        public Mesh BuildMesh() {
            Mesh mesh = new Mesh();
            mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;

            Debug.Log("Building mesh: " + vertices.Count + " vertices, " + triangles.Count + " triangles");

            mesh.SetVertices(vertices);
            mesh.SetTriangles(triangles, 0);
            mesh.SetColors(colors);

            mesh.RecalculateBounds();
            mesh.RecalculateNormals();

            return mesh;
        }

        public void AddVertex(Vector3 v) => vertices.Add(v * Scale);
        public void AddTriangle(int v) => triangles.Add(v);
        public void AddColor(Color c) => colors.Add(c);
    }
}