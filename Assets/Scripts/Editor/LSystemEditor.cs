using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Default {
    [CustomEditor(typeof(LSystem)), CanEditMultipleObjects]
    public class PlantSettingsEditor : Editor {

        #region fields
        private LSystem lsystem;

        public HashSet<object> MenuFolds { get; set; } = new HashSet<object>();

        private bool varianceFold;

        private long lastGenerateTime;
        private static int cycleIndex;
        #endregion

        public override void OnInspectorGUI() {
            GeneralPlantSettingUI();
            GrammarSettings();
        }

        private void GeneralPlantSettingUI() {
            CustomInspectorTools.CreateArea("Plant settings");
            LSystem.Axiom = CustomInspectorTools.TextArea("Axiom", LSystem.Axiom, int.MaxValue, true);
            LSystem.Iterations = CustomInspectorTools.IntegerField("Iterations", LSystem.Iterations);

            LSystem.UseSeed = CustomInspectorTools.BoolField("Use seed", LSystem.UseSeed);

            if (LSystem.UseSeed) {
                LSystem.Seed = CustomInspectorTools.IntegerSlider("Seed", LSystem.Seed, 0, int.MaxValue - 10);
            }

            LSystem.CaseSensitive = CustomInspectorTools.BoolField("Case sensitive", LSystem.CaseSensitive);

            CustomInspectorTools.EndArea();
        }

        #region grammar rules
        private int detailedRuleEditMode;
        private void ToggleEdit() {
            detailedRuleEditMode++;
            if (detailedRuleEditMode > 2) {
                detailedRuleEditMode = 0;
            }
        }

        private void GrammarSettings() {
            CustomInspectorTools.CreateArea("Grammar settings");
            // InspectorGUI.CreateBox();
            GrammarRules();
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Add character definition")) {
                LSystem.CharacterDefinitions.Add(new LSystemCharacterSetting());
            }
            if (GUILayout.Button("Clear definitions")) {
                LSystem.CharacterDefinitions.Clear();
            }

            //  GUILayout.EndVertical();
            GUILayout.EndHorizontal();
            CustomInspectorTools.EndArea();
        }

        private void GrammarRules() {
            for (int i = 0; i < LSystem.CharacterDefinitions.Count; i++) {
                RuleSetting(LSystem.CharacterDefinitions[i], i);
                GUILayout.Space(10);
            }
        }
        private void RuleSetting(LSystemCharacterSetting rule, int index) {
            CustomInspectorTools.CreateBox();

            GUILayout.BeginHorizontal();
            string charString = CustomInspectorTools.TextField("Command", "" + rule.Command);
            GUILayout.EndHorizontal();

            rule.Command = charString;

            if (rule.Command.Length == 0) {
                if (GUILayout.Button("Remove character definition")) {
                    LSystem.CharacterDefinitions.RemoveAt(index);
                }
            } else {
                GUILayout.Space(10);

                GUILayout.BeginHorizontal();
                GUILayout.Label("Rule");
                GUILayout.Label("Probability");
                GUILayout.EndHorizontal();
                for (int i = 0; i < rule.Rules.Count; i++) {
                    ProbabilityRule probRule = rule.Rules[i];
                    RuleRow(rule, i);
                }

                GUILayout.BeginHorizontal();
                if (GUILayout.Button("Add rule")) {
                    rule.Rules.Add(rule.DefaultRule);
                }
                if (GUILayout.Button("Edit")) {
                    ToggleEdit();
                }

                GUILayout.EndHorizontal();

                GUILayout.Space(10);
                if (GUILayout.Button("Remove character definition")) {
                    LSystem.CharacterDefinitions.RemoveAt(index);
                }
            }

            GUILayout.EndVertical();
        }
        private void RuleRow(LSystemCharacterSetting charDef, int index) {
            GUILayout.BeginHorizontal();
            //if (GUILayout.Button("Remove")) {
            //    charDef.Rules.RemoveAt(index);
            //}

            ProbabilityRule rule = charDef.Rules[index];

            string rString;
            if (detailedRuleEditMode == 2) {
                GUILayout.ExpandWidth(false);
                rString = EditorGUILayout.TextField(rule.Rule);

                if (GUILayout.Button("Remove")) {
                    charDef.Rules.RemoveAt(index);
                }
                //rule.Probability = InspectorGUI.FloatField("", rule.Probability, 0, 1);
                GUILayout.ExpandWidth(true);
            } else if (detailedRuleEditMode == 0) {
                rString = EditorGUILayout.TextField(rule.Rule);
                rule.Probability = CustomInspectorTools.FloatSlider("", rule.Probability, 0, 1);
            } else {
                rString = EditorGUILayout.TextArea(rule.Rule);
            }

            if (rString == null || rString.Length == 0) {
                rule.Rule = "";
            } else {
                rule.Rule = rString.ToLower();
            }

            GUILayout.EndHorizontal();
        }
        #endregion

        public LSystem LSystem {
            get {
                if (lsystem == null) {
                    lsystem = (LSystem)target;
                }
                return lsystem;
            }
        }
    }
}
