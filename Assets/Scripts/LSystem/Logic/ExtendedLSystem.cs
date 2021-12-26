using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Default {
    public class ExtendedLSystem {
        #region fields
        public const string PRIMARY_LSYSTEM_NAME = "self";
        public const char SUBSYSTEM_PREFIX = '$';

        private Dictionary<string, LSystemConfiguration> referenceSystems;

        private UniqueStringIndexer wordIndexer;
        private Dictionary<string, LSystemGrammar> grammars;

        private Dictionary<int, string> translation;

        private struct QueuedLSystem {
            public LSystemGrammar Grammar;
            public int Iterations;

            public QueuedLSystem(LSystemGrammar grammar, int iterations) {
                Grammar = grammar;
                Iterations = iterations;
            }
        }

        public struct LSystemResult {
            public List<int> Result;
            public Dictionary<int, string> Translation;
        }
        #endregion

        #region constructor
        public ExtendedLSystem(LSystemConfiguration primaryLSystem, LSystemConfiguration[] subSystemArray) {
            if (!PRIMARY_LSYSTEM_NAME.Equals(primaryLSystem.LSystemName)) {
                // todo alternate solution
                throw new NotImplementedException("The primary LSystem was not primary");
            }

            referenceSystems = new Dictionary<string, LSystemConfiguration>();
            referenceSystems.Add(PRIMARY_LSYSTEM_NAME, primaryLSystem);

            InitSubsytems(subSystemArray);
            InitGrammars();

            translation = wordIndexer.BuildTranslationTable();
        }

        private void InitSubsytems(LSystemConfiguration[] subSystemArray) {
            foreach (LSystemConfiguration conf in subSystemArray) {
                if (PRIMARY_LSYSTEM_NAME.Equals(conf.LSystemName)) {
                    throw new Exception($"Only the primary L-system can have the name \"{PRIMARY_LSYSTEM_NAME}\"");
                }
                if (referenceSystems.ContainsKey(conf.LSystemName)) {
                    throw new Exception($"The lsystem \"{conf.LSystemName}\" exists more than once in the collection");
                }

                referenceSystems.Add(conf.LSystemName, conf);
            }
        }

        private void InitGrammars() {
            wordIndexer = new UniqueStringIndexer();
            grammars = new Dictionary<string, LSystemGrammar>();
            foreach (LSystemConfiguration lsystem in referenceSystems.Values) {
                AddGrammar(lsystem.LSystemName, lsystem);
            }
        }

        private void AddGrammar(string name, LSystemConfiguration lsystem) {
            grammars.Add(name, new LSystemGrammar(wordIndexer, lsystem.Axiom, lsystem.CharacterDefinitions.ToArray()));
        }
        #endregion

        #region generation
        public string BuildString(int maxRecursionLevel) {
            maxRecursionLevel = Mathf.Max(0, maxRecursionLevel);
            List<int> result = new List<int>();
            BuildList(result, 0, maxRecursionLevel, new QueuedLSystem(grammars[PRIMARY_LSYSTEM_NAME], referenceSystems[PRIMARY_LSYSTEM_NAME].Iterations));
            Logger.LogVariables("LSystem literal count", result.Count);
            return string.Join(" ", result.Select(lit => translation[lit]));
        }

        private void BuildList(List<int> result, int depth, int maxDepth, QueuedLSystem lsystem) {
            if (depth > maxDepth) {
                return;
            }
            int[] lsystemResult = LSystem.Iterate(lsystem.Grammar, lsystem.Iterations);
            foreach (int literal in lsystemResult) {
                if (LiteralIsSubsystem(literal, out LSystemConfiguration subSystem)) {
                    BuildList(result, depth + 1, maxDepth, new QueuedLSystem(grammars[subSystem.LSystemName], subSystem.Iterations));
                    continue;
                }
                result.Add(literal);
            }
        }

        private bool LiteralIsSubsystem(int literal, out LSystemConfiguration subSystem) {
            string s = translation[literal];
            if (s[0] == SUBSYSTEM_PREFIX) {
                if (!referenceSystems.TryGetValue(s.Substring(1), out subSystem)) {
                    throw new Exception($"Tried to reference L-system which does not exist in the current context: \"{s}\"");
                }
                return true;
            }
            subSystem = null;
            return false;
        }
        #endregion
    }
}
