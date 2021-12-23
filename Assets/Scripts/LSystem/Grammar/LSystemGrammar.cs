using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Default {
    public class LSystemGrammar {
        #region fields
        private const char SPLITTER_CHAR = ' ';
        public int[] Axiom { get; private set; }

        private UniqueStringIndexer indexer;
        private LSystemCharacter[] characters;
        #endregion

        #region constructor old
        // inefficient but soon deprecated anyway, no support for aliases
        public LSystemGrammar(string axiom, LSystemCharacterSetting[] commands) {
            indexer = new UniqueStringIndexer();

            HandleString(axiom);
            foreach (LSystemCharacterSetting command in commands) {
                Logger.Log($"Command: {command.Command}");
                HandleString(command.Command);
                foreach (ProbabilityRule r in command.Rules) {
                    Logger.Log($"    Rule: {r.Rule}");
                    HandleString(r.Rule);
                }
            }

            characters = new LSystemCharacter[indexer.UniqueCount];

            foreach (var charIndex in indexer) {
                try {
                    // is command
                    LSystemCharacterSetting command = commands.First(c => c.Command.Equals(charIndex.Key));

                    LSystemCharacter character = new LSystemCharacter(charIndex.Key, charIndex.Value, command.Rules.Select(r => {
                        int[] ruleString = r.Rule.Split(SPLITTER_CHAR).Where(s => s.Length > 0).Select(s => indexer.SetAndGetIndex(s)).ToArray();
                        return new LSystemRule(r.Probability, ruleString);
                    }).ToArray());

                    characters[charIndex.Value] = character;
                } catch (System.Exception e) {
                    // not command, by default does not change in iteration
                    LSystemCharacter character = new LSystemCharacter(charIndex.Key, charIndex.Value, new LSystemRule(0, charIndex.Value));
                    characters[charIndex.Value] = character;
                }
            }

            List<int> axiomlist = new List<int>();
            foreach (string word in axiom.Split(SPLITTER_CHAR).Where(s => s.Length > 0)) {
                axiomlist.Add(indexer.SetAndGetIndex(word));
            }
            Axiom = axiomlist.ToArray();

            void HandleString(string str) {
                foreach (string word in str.Split(SPLITTER_CHAR).Where(s => s.Length > 0)) {
                    if (word.Length == 0) {
                        continue;
                    }

                    indexer.SetAndGetIndex(word);
                }
            }
        }
        #endregion

        public string LiteralToString(int literal) => characters[literal].Name;

        public List<int> PerformTransformation(List<int> literals) {
            List<int> newLiterals = new List<int>(literals.Count);

            foreach (int literal in literals) {
                if (!GetCharacter(literal, out LSystemCharacter character)) {
                    throw new System.Exception("Undefined character");
                }

                foreach (int newLiteral in character.GetRule(RNG.Float).Transformation) {
                    newLiterals.Add(newLiteral);
                }
            }

            return newLiterals;
        }

        public string ConvertToString(int[] literals) {
            StringBuilder sb = new StringBuilder();
            foreach (int literal in literals) {
                if (!GetCharacter(literal, out LSystemCharacter character)) {
                    throw new System.Exception("Undefined character");
                }
                sb.Append($"{character.Name} ");
            }
            return sb.ToString();
        }

        private bool GetCharacter(int literal, out LSystemCharacter character) {
            if (literal >= characters.Length) {
                character = default;
                return false;
            }
            character = characters[literal];
            return true;
        }
    }
}
