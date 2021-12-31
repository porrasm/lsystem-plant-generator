using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Default {
    [Serializable]
    public class LSystemCharacterSetting : ISetting {
        #region fields
        [field: SerializeField]
        public string Command { get; set; }
        [field: SerializeField]
        public List<ProbabilityRule> Rules { get; set; }
        [field: SerializeField]
        public bool IsAlias { get; set; }
        #endregion

        public LSystemCharacterSetting() {
            Rules = new List<ProbabilityRule>();
            Command = "";
        }

        public ProbabilityRule DefaultRule {
            get {
                ProbabilityRule rule = new ProbabilityRule();
                rule.Rule = Command;
                return rule;
            }
        }


        public void Validate() {
            Command.ToLower();
            foreach (ProbabilityRule r in Rules) {
                r.Rule = r.Rule.ToLower();
            }
        }
    }
}
