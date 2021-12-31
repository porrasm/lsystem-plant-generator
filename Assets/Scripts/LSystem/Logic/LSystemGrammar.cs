using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Default {
    public class LSystemGrammar {
        #region fields
        public const char SPLITTER_CHAR = ' ';
        public const char COMMAND_START = '"';
        public const char COMMAND_END = '"';

        public int[] Axiom { get; private set; }

        private Dictionary<int, LSystemCharacter> characters;
        #endregion

        #region constructor old
        // inefficient but soon deprecated anyway, no support for aliases
        // EL SPAGHETTO!! PLEASO FIXO! REQUIRO REFACTORO OF INPUT TYPE!
        public LSystemGrammar(UniqueStringIndexer indexer, string axiom, LSystemCharacterSetting[] commands) {
            ValidateAxiomOrCommand(axiom, true);

            UniqueStringIndexer localIndexer = new UniqueStringIndexer();

            RegisterStringIndex(axiom);
            foreach (LSystemCharacterSetting command in commands) {
                Logger.Log($"Command: {command.Command}");
                ValidateAxiomOrCommand(command.Command);
                RegisterStringIndex(command.Command);
                foreach (ProbabilityRule r in command.Rules) {
                    Logger.Log($"    Rule: {r.Rule}");
                    RegisterStringIndex(r.Rule);
                }
            }

            characters = new Dictionary<int, LSystemCharacter>();

            foreach (KeyValuePair<string, int> localCharIndex in localIndexer) {
                int charIndex = indexer[localCharIndex.Key];
                try {
                    // is command
                    LSystemCharacterSetting command = commands.First(c => c.Command.Equals(localCharIndex.Key));

                    LSystemCharacter character = new LSystemCharacter(localCharIndex.Key, charIndex, command.IsAlias, command.Rules.Select(r => {
                        int[] ruleString = GetLSystemWords(r.Rule).Where(s => s.Length > 0).Select(s => indexer.SetAndGetIndex(s)).ToArray();
                        return new LSystemRule(r.Probability, ruleString);
                    }).ToArray());

                    characters.Add(charIndex, character);
                } catch (System.Exception e) {
                    // not command, by default does not change in iteration
                    LSystemCharacter character = new LSystemCharacter(localCharIndex.Key, charIndex, false, new LSystemRule(0, charIndex));
                    characters.Add(charIndex, character);
                }
            }

            List<int> axiomlist = new List<int>();
            foreach (string word in GetLSystemWords(axiom).Where(s => s.Length > 0)) {
                axiomlist.Add(indexer.SetAndGetIndex(word));
            }
            Axiom = axiomlist.ToArray();

            void RegisterStringIndex(string str) {
                foreach (string word in GetLSystemWords(str).Where(s => s.Length > 0)) {
                    if (word.Length == 0) {
                        continue;
                    }

                    localIndexer.SetAndGetIndex(word);
                    indexer.SetAndGetIndex(word);
                }
            }
        }
        #endregion

        public string LiteralToString(int literal) => characters[literal].Name;

        public List<int> PerformTransformation(List<int> literals, bool transformAliases) {
            List<int> newLiterals = new List<int>(literals.Count);

            foreach (int literal in literals) {
                if (!GetCharacter(literal, out LSystemCharacter character)) {
                    throw new System.Exception("Undefined character");
                }

                if (character.IsAlias && !transformAliases) {
                    newLiterals.Add(literal);
                    continue;
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
            if (characters.TryGetValue(literal, out character)) {
                return true;
            }
            return false;
        }

        #region utility
        public static void ValidateAxiomOrCommand(string axiom, bool allowCommand = false) {
            if (axiom == null || axiom.Length == 0) {
                throw new System.Exception("Axiom cannot be null or empty");
            }
            if (!allowCommand && (axiom.Contains(COMMAND_START) || axiom.Contains(COMMAND_END))) {
                throw new System.Exception("Axiom cannot contain commands");
            }
        }

        public static bool WordIsCommand(string word) {
            return word[0] == COMMAND_START && word[word.Length - 1] == COMMAND_END;
        }

        public static List<string> GetLSystemWords(string plant) {
            return Parser.ParseWords(plant, SPLITTER_CHAR, COMMAND_START, COMMAND_END);
        }
        #endregion
    }
}
