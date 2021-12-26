using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Default {
    // editable at runtime, potential problems, copy? struct?
    [Serializable]
    public class LSystemConfiguration {
        #region fields
        [field: SerializeField]
        public string LSystemName { get; set; }

        [field: SerializeField]
        public string Axiom { get; set; }

        [field: SerializeField]
        public bool UseSeed { get; set; }

        [field: SerializeField]
        public int Seed { get; set; }

        [field: SerializeField]
        public int Iterations { get; set; }

        [field: SerializeField]
        public bool CaseSensitive { get; set; }

        [field: SerializeField]
        public List<LSystemCharacterSetting> CharacterDefinitions { get; set; }
        #endregion
    }
}
