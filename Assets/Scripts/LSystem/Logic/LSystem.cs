using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Default {
    public class LSystem {
        #region fields
        public string Name { get; private set; }
        public LSystemGrammar Grammar { get; private set; }
        #endregion

        public LSystem(string name, LSystemGrammar grammar) { 
            Name = name;
            Grammar = grammar;
        }

        #region iterations
        public static int[] Iterate(Iterator i) {
            return Iterate(i.Grammar, i.Iterations);
        }
        public static int[] Iterate(LSystemGrammar grammar, int iterationCount) {
            Logger.Log($"LSystem iterate {iterationCount}");
            List<int> iteration = new List<int>(grammar.Axiom);
            for (int i = 0; i < iterationCount; i++) {
                iteration = grammar.PerformTransformation(iteration);
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
