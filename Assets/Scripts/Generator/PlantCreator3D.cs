using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace Default {
    public class PlantCreator3D : MonoBehaviour {
        #region fields
        [field: SerializeField]
        public LSystemConfigurer LSystemConf { get; set; }

        [field: SerializeField]
        public int MaxLSystemRecursionLevel { get; set; } = 5;

        [field: SerializeField]
        public MeshFilter TargetMesh { get; set; }

        [field: SerializeField]
        public bool GenerateOnStart { get; set; }

        [SerializeField]
        private bool regenerateOnChange;

        [field: SerializeField]
        public int VerticesPerMeter { get; set; }

        [field: SerializeField]
        public PlantGeneratorSettings3D InitialSettings { get; set; }
        #endregion

        private void Start() {
            if (GenerateOnStart) { 
                SetAndGenerateMesh();
            }
        }

        private void OnValidate() {
            if (regenerateOnChange) {
                SetAndGenerateMesh();
            }
        }

        #region generation
        public Mesh GenerateMesh() {
           return GenerateMesh(LSystemConf.BuildLSystem(), InitialSettings, VerticesPerMeter, MaxLSystemRecursionLevel);
        }

        public void SetAndGenerateMesh() {
            TargetMesh.mesh = GenerateMesh();
        }

        public static Mesh GenerateMesh(ExtendedLSystem lsystem, PlantGeneratorSettings3D initialSettings, int verticesPerMeter, int maxRecursionLevel, bool useDefaultGenerationRules = true, ICharacterRule[] additionalRules = null) {
            // inefficient string usage, use alternate int based solution
            string plantString = lsystem.BuildString(maxRecursionLevel);
            Logger.LogVariables("plantString", plantString);
            MeshGenerator3D generator = new MeshGenerator3D(useDefaultGenerationRules, additionalRules);
            return generator.Generate(plantString, initialSettings, verticesPerMeter);
        }
        #endregion
    }
}
