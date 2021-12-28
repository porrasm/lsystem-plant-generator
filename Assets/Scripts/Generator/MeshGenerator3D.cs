using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Default {
    public class MeshGenerator3D {
        #region fields
        public const bool FailIfUnknownRule = true;
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
        public Mesh Generate(string plantString, PlantGeneratorSettings3D initialSettings, int pointsPerMeter) {
            PlantDensityMap plant = GeneratePlantRepresentation(plantString, initialSettings, pointsPerMeter);
            return plant.GenerateMesh();
        }

        private PlantDensityMap GeneratePlantRepresentation(string plantString, PlantGeneratorSettings3D initialSettings, int pointsPerMeter) {
            PlantDensityMap plant = new PlantDensityMap(pointsPerMeter);
            Stack<TurtleState> stateStack = new Stack<TurtleState>();

            TurtleState state = new TurtleState() {
                Forward = Vector3.up,
                Settings = initialSettings
            };
            PlantBranching<TurtleState> branching = new PlantBranching<TurtleState>(() => stateStack.Push(state), (s) => stateStack.Push(s), stateStack.Pop);

            foreach (string rule in plantString.Split()) {
                if (rule.Length == 0) {
                    continue;
                }

                Logger.Log($"RUle: {rule}");
                if (rules.TryGetValue(rule, out ICharacterRule characterRule)) {
                    Logger.Log(characterRule.Description);
                    characterRule.Apply(plant, branching, ref state);
                } else if (FailIfUnknownRule) {
                    throw new Exception($"Unknow rule encountered: {rule}");
                }
            }

            return plant;
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
