using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Default {
    // editable at runtime, potential problems, copy? struct?
    [Serializable]
    public class LSystemConfiguration {
        #region fields
        public enum ConfigurationType {
            LSystem,
            Alias
        }

        [field: SerializeField]
        public string LSystemName { get; set; } = "name";
        [field: SerializeField]
        public ConfigurationType Type { get; set; }

        [field: SerializeField]
        public string Axiom { get; set; } = "f";

        [field: SerializeField]
        public bool UseSeed { get; set; } = false;

        [field: SerializeField]
        public int Seed { get; set; }

        [field: SerializeField]
        public int Iterations { get; set; } = 1;

        [field: SerializeField]
        public bool CaseSensitive { get; set; } = false;

        [field: SerializeField]
        public List<LSystemCharacterSetting> CharacterDefinitions { get; set; }
        #endregion
    }
}
