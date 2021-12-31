using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Default {
    public struct Cylinder {
        #region fields
        public Vector3 P1 { get; }
        public Vector3 P2 { get; }
        public float Radius { get; }
        private readonly float h2;
        #endregion

        public Cylinder(Vector3 a, Vector3 b, float radius) {
            P1 = a;
            P2 = b;
            Radius = radius;
            h2 = Vector3.Dot(P2 - P1, P2 - P1);
        }

        public bool Contains(Vector3 p) {
            //if
            // dot(P - P1, P - P1) - dot(P - P1, P2 - P1) ^ 2 / H2 <= R ^ 2 & ...
            // dot(P - P1, P2 - P1) >= 0 &
            // dot(P - P2, P2 - P1) <= 0
            return
               Vector3.Dot(p - P1, p - P1) - (Mathf.Pow(Vector3.Dot(p - P1, P2 - P1), 2) / h2) <= Mathf.Pow(Radius, 2)
            && Vector3.Dot(p - P1, P2 - P1) >= 0
            && Vector3.Dot(p - P2, P2 - P1) <= 0;
        }

        public override string ToString() => $"[{P1}, {P2}, {Radius}]";
    }
}
