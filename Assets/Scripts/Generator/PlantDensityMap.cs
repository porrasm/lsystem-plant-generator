using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Default {
    public class PlantDensityMap {
        #region fields
        public int PointsPerMeter { get; private set; }
        public float PointDistance { get; private set; }

        public ExtendableArray3D<DensityPoint> Density { get; private set; } = new ExtendableArray3D<DensityPoint>(() => default);
        #endregion

        public PlantDensityMap(int pointsPerMeter) {
            PointsPerMeter = pointsPerMeter;
            PointDistance = 1.0f / pointsPerMeter;
        }
        
        public void DrawLine(Vector3 start, Vector3 end, float thickness) {
            Logger.Log($"Draw line from {start} to {end} with thickness {thickness}");
            Debug.DrawLine(start, end);
            Vector3Int[] indexes = GetIndexesAroundLine(start, end, thickness);
            foreach (Vector3Int index in indexes) {
               // AddPoint(index, 1);
               // continue;
                Vector3 pos = PositionFromIndex(index);
                if (PointDistanceFromLine(pos, start, end) is var distance && distance <= thickness) {
                    // do something
                    float value = 1 - Matht.Percentage(0, thickness, distance);
                    AddPoint(index, value);
                }
            }
        }

        private void AddPoint(Vector3Int point, float value) {
            Debug.DrawLine(point, point + Vector3.forward * 0.05f, Color.red);
            DensityPoint prev = Density[point];
            prev.Density = Mathf.Max(prev.Density, value);
            Density[point] = prev;
        }

        // Maybe inefficient?
        private Vector3Int[] GetIndexesAroundLine(Vector3 start, Vector3 end, float radius) {
            HashSet<Vector3Int> indexes = new HashSet<Vector3Int>();

            Vector3 lineDirection = end - start;

            radius = Mathf.Max(radius, PointDistance);

            float lineLength = Vector3.Distance(start, end);
            float incrementAmount = Mathf.Sqrt(2) * radius;
            incrementAmount = radius;

            for (int i = 0; i * incrementAmount is var currentDistance && currentDistance < lineLength; ++i) {
                Vector3 center = start + (currentDistance * lineDirection);
                Bounds box = new Bounds(center, new Vector3(radius, radius, radius));
                IndexesFromBounds(box, indexes);
            }

            return indexes.ToArray();
        }

        public float PointDistanceFromLine(Vector3 point, Vector3 start, Vector3 end) {
            Vector3 d = (end - start) / Vector3.Distance(end, start);
            Vector3 v = point - start;
            float t = Vector3.Dot(v, d);
            Vector3 P = start + (t * d);
            return Vector3.Distance(P, point);
        }

        private void IndexesFromBounds(Bounds bounds, HashSet<Vector3Int> indexes) {
            Vector3Int startIndex = IndexFromPosition(bounds.min);
            Vector3Int endIndex = IndexFromPosition(bounds.max);

            for (int x = startIndex.x; x <= endIndex.x; ++x) {
                for (int y = startIndex.y; y <= endIndex.y; ++y) {
                    for (int z = startIndex.z; z <= endIndex.z; ++z) {
                        indexes.Add(new Vector3Int(x, y, z));
                    }
                }
            }
        }

        public Vector3Int IndexFromPosition(Vector3 position) {
            position *= PointsPerMeter;
            return new Vector3Int((int)position.x, (int)position.y, (int)position.z);
        }
        public Vector3 PositionFromIndex(Vector3Int index) {
            return new Vector3(PointDistance * index.x, PointDistance * index.y, PointDistance * index.z);
        }
    }
}
