using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Default {
    public struct LSystemRule {
        public float Probability;
        public int[] Transformation;

        public LSystemRule(float probability, params int[] transformation) {
            Probability = probability;
            Transformation = transformation;
        }
    }
}
