using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Default {
    public class LSystem : MonoBehaviour {
        #region fields
        [SerializeField]
        private string lsystemName;
        public string LsystemName { get => lsystemName; set => lsystemName = value; }


        [SerializeField]
        private string axiom;
        public string Axiom { get => axiom; set => axiom = value; }

        [SerializeField]
        private bool useSeed;
        public bool UseSeed { get => useSeed; set => useSeed = value; }

        [SerializeField]
        private int seed;
        public int Seed { get => seed; set => seed = value; }

        [SerializeField]
        private int iterations;
        public int Iterations { get => iterations; set => iterations = value; }

        [SerializeField]
        private bool caseSensitive;
        public bool CaseSensitive { get => caseSensitive; set => caseSensitive = value; }

        [SerializeField]
        private List<LSystemCharacterSetting> characterDefinitions;
        public List<LSystemCharacterSetting> CharacterDefinitions { get => characterDefinitions; }
        #endregion
    }
}
