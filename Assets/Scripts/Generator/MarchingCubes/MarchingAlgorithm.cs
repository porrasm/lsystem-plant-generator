using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace Default {
    public abstract class Marching {

        #region fields
        public float Surface { get; set; }
        protected DensityPoint[] cube;

        /// <summary>
        /// Winding order of triangles use 2,1,0 or 0,1,2
        /// </summary>
        protected int[] triangleOrder;

        protected MarchingState state;

        public delegate bool VoxelIsActiveDelegate(float voxelValue);
        public VoxelIsActiveDelegate VoxelIsActive { get; set; }
        #endregion


        public Marching(float surface = 0.5f) {
            Surface = surface;
            cube = new DensityPoint[8];

            if (Surface > 0) {
                triangleOrder = new int[] { 0, 1, 2 };
            } else {
                triangleOrder = new int[] { 2, 1, 0 };
            }

            VoxelIsActive = (val) => {
                return val <= Surface;
            };
        }

        public virtual void Generate(MarchingState state) {
            this.state = state;

            Vector3Int len = state.Values.Length;

            for (int x = 0; x < len.x - 1; x++) {
                for (int y = 0; y < len.y - 1; y++) {
                    for (int z = 0; z < len.z - 1; z++) {

                        // Get the values in the 8 neighbours which make up a cube
                        for (int i = 0; i < 8; i++) {
                            int ix = x + VertexOffset[i, 0];
                            int iy = y + VertexOffset[i, 1];
                            int iz = z + VertexOffset[i, 2];

                            cube[i] = state.Values.Arr[ix + (iy * len.x) + (iz * len.x * len.y)];
                        }

                        // Perform algorithm
                        March(x, y, z);
                    }
                }
            }

            this.state = null;
        }

        /// <summary>
        /// MarchCube performs the Marching algorithm on a single cube
        /// </summary>
        protected abstract void March(int x, int y, int z);

        /// <summary>
        /// GetOffset finds the approximate point of intersection of the surface
        /// between two points with the values v1 and v2
        /// </summary>
        //protected virtual float GetOffset(float v1, float v2) {
        //    float delta = v2 - v1;
        //    return (delta == 0.0f) ? Surface : (Surface - v1) / delta;
        //}
        protected virtual float GetOffset(float v1, float v2) {
            float delta = v2 - v1;
            return (delta == 0.0f) ? Surface : (Surface - v1) / delta;
        }

        /// <summary>
        /// VertexOffset lists the positions, relative to vertex0, 
        /// of each of the 8 vertices of a cube.
        /// vertexOffset[8][3]
        /// </summary>
        protected static readonly int[,] VertexOffset = new int[,]
        {
            {0, 0, 0},{1, 0, 0},{1, 1, 0},{0, 1, 0},
            {0, 0, 1},{1, 0, 1},{1, 1, 1},{0, 1, 1}
        };
    }
}
