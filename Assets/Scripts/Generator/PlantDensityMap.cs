using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Default {
    public class PlantDensityMap {
        #region fields
        public int PointsPerMeter { get; private set; }
        public float PointDistance { get; private set; }

        public ExtendableArray3D<DensityPoint> Density { get; private set; } = new ExtendableArray3D<DensityPoint>(() => new DensityPoint(0, new Color(0, 0, 0, 0)));
        #endregion

        public PlantDensityMap(int pointsPerMeter) {
            PointsPerMeter = pointsPerMeter;
            PointDistance = 1.0f / pointsPerMeter;
        }

        #region new
        public void DrawSphere(Vector3 location, float radius, float density, Color color) {
            Vector3 center = location * PointsPerMeter;
            Bounds bounds = new Bounds(center, Vector3.one * radius * PointsPerMeter);

            float maxDistance = radius * PointsPerMeter;

            for (int x = (int)bounds.min.x; x <= bounds.max.x; x++) {
                for (int y = (int)bounds.min.y; y <= bounds.max.y; y++) {
                    for (int z = (int)bounds.min.z; z <= bounds.max.z; z++) {
                        Vector3Int point = new Vector3Int(x, y, z);
                        float distanceFromSphereCenter = Vector3.Distance(point, center);
                        if (distanceFromSphereCenter > maxDistance) {
                            continue;
                        }

                        float value = 1 - Matht.Percentage(0, maxDistance, distanceFromSphereCenter);
                        value *= ThinFactor(density);

                        AddPoint(point, value, color);
                    }
                }
            }
        }

        public void DrawLine(Vector3 start, Vector3 end, Color c, float thickness, float density) {
            Cylinder cylinder = new Cylinder(start * PointsPerMeter, end * PointsPerMeter, thickness * PointsPerMeter);

            // Logger.Log($"Draw line from {start} to {end} with thickness ({thickness})");
            // Debug.DrawLine(start, end, Color.red, 10);

            foreach (Vector3Int index in Matht.GetAllPointsInCylinder(cylinder)) {
                // AddPoint(index, 1);
                // continue;
                Vector3 pos = PositionFromIndex(index);

                float dis = PointDistanceFromLine(pos, start, end);
                if (dis > thickness) {
                    continue;
                }

                if (PointDistanceFromLine(pos, start, end) is var distance && distance <= thickness) {
                    // do something
                    float value = 1 - Matht.Percentage(0, thickness, distance);
                    value *= ThinFactor(density);
                    AddPoint(index, value, c);
                }
            }
        }

        private float ThinFactor(float density) {
            return RNG.Range(density, 1);
        }
        #endregion


        public void AddPoint(Vector3Int point, float value, Color c) {
            Vector3 debugPoint = point;
            debugPoint *= 0.2f;
            debugPoint += new Vector3Int(5, 0, 5);
            // Debug.DrawLine(debugPoint, debugPoint + Vector3.forward * 0.1f, Matht.Interpolate(new Color(0.5f, 0, 0, 1), Color.green, value, v => v), 10);
            DensityPoint prev = Density[point];
            prev.Density = Mathf.Max(prev.Density, value);
            prev.Color = c;
            Density[point] = prev;
        }

        public float PointDistanceFromLine(Vector3 point, Vector3 start, Vector3 end) {
            float min = Mathf.Min(Vector3.Distance(point, start), Vector3.Distance(point, end));
            Vector3 d = (end - start) / Vector3.Distance(end, start);
            Vector3 v = point - start;
            float t = Vector3.Dot(v, d);
            Vector3 P = start + (t * d);
            return Mathf.Min(min, Vector3.Distance(P, point));
        }

        public Vector3Int IndexFromPositionMin(Vector3 position) {
            position *= PointsPerMeter;
            return new Vector3Int((int)position.x, (int)position.y, (int)position.z);
        }
        public Vector3 PositionFromIndex(Vector3Int index) {
            return new Vector3(PointDistance * index.x, PointDistance * index.y, PointDistance * index.z);
        }

        public Mesh GenerateMesh() {
            MarchingCubes marching = new MarchingCubes(0.5f);
            DensityPoint[,,] arr3D = Density.To3DArray(out Vector3Int origo, (d) => d);

            MarchingState state = new MarchingState(new Array3D<DensityPoint>(arr3D), PointDistance);
            marching.Generate(state);

            return state.BuildMesh();
        }
    }
}
