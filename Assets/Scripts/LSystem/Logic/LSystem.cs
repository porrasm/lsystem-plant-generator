using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Default {
    public static class LSystem {
        #region iterations
        public static int[] Iterate(Iterator i) {
            return Iterate(i.Grammar, i.Iterations);
        }
        public static int[] Iterate(LSystemGrammar grammar, int iterationCount) {
            List<int> iteration = new List<int>(grammar.Axiom);
            for (int i = 0; i < iterationCount; i++) {
                bool isLastIteration = i == iterationCount - 1;
                iteration = grammar.PerformTransformation(iteration, isLastIteration);
            }
            return iteration.ToArray();
        }
        #endregion

        public struct Iterator {
            public LSystemGrammar Grammar;
            public int Iterations;

            public Iterator(LSystemGrammar grammar, int iterations) {
                Grammar = grammar;
                Iterations = iterations;
            }
        }
    }
}
