using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Default {
    public class MeshGenerator3D {
        #region fields
        private Dictionary<string, ICharacterRule> rules;
        #endregion

        public MeshGenerator3D(bool useDefaultRules, ICharacterRule[] additionalRules) {
            List<ICharacterRule> ruleSet = useDefaultRules ? GetDefaultCharacters() : new List<ICharacterRule>();
            if (additionalRules != null) {
                foreach (ICharacterRule rule in additionalRules) {
                    ruleSet.Add(rule);
                }
            }

            if (ruleSet.Count == 0) {
                throw new Exception("Ruleset was empty");
            }

            rules = new Dictionary<string, ICharacterRule>();
            foreach (ICharacterRule rule in ruleSet) {
                if (rules.ContainsKey(rule.Character)) {
                    throw new Exception("Duplicate rule found");
                }
                rules.Add(rule.Character, rule);
            }
        }

        #region generate
        public Mesh Generate(string plantString) {
            PlantDensityMap plant = GeneratePlantRepresentation(plantString);
            return GeneratePlantMesh(plant);
        }

        private PlantDensityMap GeneratePlantRepresentation(string plantString) {
            PlantDensityMap plant = new PlantDensityMap(10);
            Stack<TurtleState> stateStack = new Stack<TurtleState>();

            TurtleState state = new TurtleState() {
                LineLength = 1f,
                LineWidth = 0.2f,
                EulerAngles = new Vector3(90f, 0f, 0f)
            };
            PlantBranching<TurtleState> branching = new PlantBranching<TurtleState>(() => stateStack.Push(state), (s) => stateStack.Push(s), stateStack.Pop);

            foreach (string rule in plantString.Split()) {
                Logger.Log($"RUle: {rule}");
                if (rules.TryGetValue(rule, out ICharacterRule characterRule)) {
                    Logger.Log(characterRule.Description);
                    characterRule.Apply(plant, branching, ref state);
                }
            }

            return plant;
        }

        private Mesh GeneratePlantMesh(PlantDensityMap plant) {
            MarchingCubes marching = new MarchingCubes(0.5f);
            plant.Density.ExtendRanges();
            float[,,] arr3D = plant.Density.To3DArray(out Vector3Int origo, (d) => d.Density);

            MarchingState state = new MarchingState(new Array3D<float>(arr3D));
            marching.Generate(state);

            return state.BuildMesh();
        }
        #endregion

        #region utility
        private static List<ICharacterRule> GetDefaultCharacters() {
            List<ICharacterRule> rules = new List<ICharacterRule>();

            rules.Add(new DefaultPlantRules.Forward());
            rules.Add(new DefaultPlantRules.Branch());
            rules.Add(new DefaultPlantRules.Debranch());
            rules.Add(new DefaultPlantRules.XAngleAdd());
            rules.Add(new DefaultPlantRules.XAngleRemove());
            rules.Add(new DefaultPlantRules.YAngleAdd());
            rules.Add(new DefaultPlantRules.YAngleRemove());

            return rules;
        }
        #endregion
    }
}
