using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Default {
    public struct LSystemCharacter {
        #region fields
        public string Name;
        public int Index;
        public LSystemRule[] Rules { get; private set; }
        private float probabilitySum;
        #endregion

        public LSystemCharacter(string name, int index, params LSystemRule[] rules) {
            Name = name;
            Index = index;
            Rules = rules;
            probabilitySum = 0;
            foreach (LSystemRule rule in rules) {
                probabilitySum += rule.Probability;
            }
        }

        public LSystemRule GetRule(float probability) {
            float adjustedProbability = probability * probabilitySum;
            float sum = 0;

            foreach (LSystemRule rule in Rules) {
                sum += rule.Probability;
                if (sum >= adjustedProbability) {
                    return rule;
                }
            }

            return Rules[Rules.Length - 1];
        }
    }
}

