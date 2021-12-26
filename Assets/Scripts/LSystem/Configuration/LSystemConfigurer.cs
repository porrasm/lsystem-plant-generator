using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Default {
    [DisallowMultipleComponent]
    public class LSystemConfigurer : MonoBehaviour {
        #region fields
        [field: SerializeField]
        public LSystemConfiguration PrimaryLSystem { get; set; }

        [field: SerializeField]
        public List<LSystemBank> SubSystems { get; set; }
        #endregion

        public ExtendedLSystem BuildLSystem() {
            return new ExtendedLSystem(PrimaryLSystem, SubSystems.Where(s => s != null).SelectMany(s => s.LSystems).ToArray());
        }
    }
}
