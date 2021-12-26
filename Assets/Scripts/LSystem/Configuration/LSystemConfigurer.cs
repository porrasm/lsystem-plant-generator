using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Default {
    public class LSystemConfigurer : MonoBehaviour {
        #region fields
        [field: SerializeField]
        public LSystemConfiguration PrimaryLSystem { get; set; }

        [field: SerializeField]
        public List<LSystemBank> SubSystems { get; set; }
        #endregion

        public ExtendedLSystem BuildLSystem() {
            return new ExtendedLSystem(PrimaryLSystem, SubSystems.SelectMany(s => s.LSystems).ToArray());
        }
    }
}
