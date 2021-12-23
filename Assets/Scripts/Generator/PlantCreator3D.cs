using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Default {
    public class PlantCreator3D : MonoBehaviour {
        #region fields
        [field: SerializeField]
        public LSystemConfiguration LSystemConf { get; set; }

        [field: SerializeField]
        public MeshFilter TargetMesh { get; set; }

        [field: SerializeField]
        public bool GenerateOnStart { get; set; }
        #endregion

        private void Start() {
            if (GenerateOnStart) { 
                SetAndGenerateMesh();
            }
        }

        #region generation
        public Mesh GenerateMesh() {
            return GenerateMesh(LSystemConf.ConvertToGrammar(), LSystemConf.Iterations);
        }

        public void SetAndGenerateMesh() {
            TargetMesh.mesh = GenerateMesh();
        }

        public static Mesh GenerateMesh(LSystemGrammar grammar, int iterations, bool useDefaultGenerationRules = true, ICharacterRule[] additionalRules = null) { 
            int[] plant = LSystem.Iterate(grammar, iterations);
            string plantString = grammar.ConvertToString(plant);

            MeshGenerator3D generator = new MeshGenerator3D(useDefaultGenerationRules, additionalRules);
            return generator.Generate(plantString);
        }
        #endregion
    }
}
