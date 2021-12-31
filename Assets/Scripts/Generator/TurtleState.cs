using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Default {
    [Serializable]
    public struct TurtleState {
        #region fields
        public Vector3 Position;
        public Vector3 Forward;

        public float AngleX;
        public float AngleY;

        public float LineLength;
        public float LineWidth;

        public float Density;

        public Color Color;
        #endregion
    }
}
