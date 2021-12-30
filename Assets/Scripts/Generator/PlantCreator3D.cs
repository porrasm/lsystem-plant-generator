using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace Default {
    [DisallowMultipleComponent]
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
            //return CircleMap(1, 0.5f).GenerateMesh();
            return generator.Generate(plantString, initialSettings, verticesPerMeter);
        }
        #endregion

        private static PlantDensityMap CircleMap(float radius, float surface) {
            PlantDensityMap plant = new PlantDensityMap(10);
            radius *= plant.PointsPerMeter;
            int max = (int)Mathf.Ceil(2 * radius);

            Vector3 center = new Vector3(radius, radius, radius);
            float maxDistance = radius * Mathf.Sqrt(2);

            for (int x = 0; x <= max; x++) {
                for (int y = 0; y <= max; y++) {
                    for (int z = 0; z <= max; z++) {
                        Vector3 pos = new Vector3(x, y, z);
                        float dis = Vector3.Distance(pos, center);
                        float perc = 1 - Matht.Percentage(0, maxDistance, dis);
                        plant.AddPoint(new Vector3Int(x, y, z), perc, Color.red);
                    }
                }
            }

            return plant;
        }
    }
}
