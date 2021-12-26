using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Default {
    [DisallowMultipleComponent]
    public class LSystemBank : MonoBehaviour {
        #region fields
        [field: SerializeField]
        public List<LSystemConfiguration> LSystems { get; set; }
        #endregion
    }
}
