using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Default {
    [Serializable]
    public class ProbabilityRule {
        public float Probability;
        public string Rule;
        public double ProbabilityRange { get; set; }
    }
}
