using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Default {
    public static class LSystem {
        #region iterations
        public static int[] Iterate(LSystemGrammar grammar, int iterationCount) {
            List<int> iteration = new List<int>(grammar.Axiom);
            for (int i = 0; i < iterationCount; i++) {
                iteration = grammar.PerformTransformation(iteration);
            }
            return iteration.ToArray();
        }
        #endregion
    }
}
