using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Default {
    public struct DensityPoint {
        public float Density { get; set; }
        public Color Color { get; set; }

        public DensityPoint(float density, Color color) {
            Density = density;
            Color = color;
        }
    }
}
