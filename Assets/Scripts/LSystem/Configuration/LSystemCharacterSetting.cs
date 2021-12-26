using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Default {
    [Serializable]
    public class LSystemCharacterSetting : ISetting {

        #region fields
        [SerializeField]
        private string command;

        [SerializeField]
        private List<ProbabilityRule> rules;

        public string Command { get => command; set => command = value; }
        public List<ProbabilityRule> Rules { get => rules; set => rules = value; }
        #endregion

        public LSystemCharacterSetting() {
            rules = new List<ProbabilityRule>();
            command = "";
        }

        public ProbabilityRule DefaultRule {
            get {
                ProbabilityRule rule = new ProbabilityRule();
                rule.Rule = command;
                return rule;
            }
        }

        public void RemoveRule(int index) {
            rules.RemoveAt(index);
            if (rules.Count == 0) {
                rules.Add(DefaultRule);
            }
        }

        public void Validate() {
            command.ToLower();
            foreach (ProbabilityRule r in rules) {
                r.Rule = r.Rule.ToLower();
            }
        }
    }
}
