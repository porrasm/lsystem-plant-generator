using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Default {
    [Serializable]
    public struct PlantGeneratorSettings3D {
        // =, +=, -=, *=, /=, %=
        // use value interface with func "GetValue()" etc.
        // integrate variation into this to separate mesh drawing and representation generation
        // variation editing disabled sadly for now
        #region fields
        public float AngleX;
        public float AngleY;

        public float LineLength;
        public float LineWidth;

        public Color Color;
        #endregion
    }
}
