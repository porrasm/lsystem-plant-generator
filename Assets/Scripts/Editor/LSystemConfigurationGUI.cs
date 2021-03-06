using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Default {
    public class LSystemConfigurationGUI {
        #region fields
        public bool Fold;
        private bool isPrimary;
        private bool iterationToggle;
        public LSystemConfiguration LSystem { get; private set; }
        #endregion

        public LSystemConfigurationGUI(LSystemConfiguration lSystem, bool isPrimary) {
            LSystem = lSystem;
            this.isPrimary = isPrimary;
        }

        public void CreateGUI() {
            if (LSystem.CharacterDefinitions == null) {
                LSystem.CharacterDefinitions = new List<LSystemCharacterSetting>();
            }
            GeneralPlantSettingUI();

            if (LSystem.Type == LSystemConfiguration.ConfigurationType.LSystem) {
                GrammarSettings();
            }
        }

        private void GeneralPlantSettingUI() {
            CustomInspectorTools.CreateArea("Plant settings");
            LSystem.LSystemName = CustomInspectorTools.TextField("System name", LSystem.LSystemName);
            if (isPrimary) {
                LSystem.LSystemName = ExtendedLSystem.PRIMARY_LSYSTEM_NAME;
                LSystem.Type = LSystemConfiguration.ConfigurationType.LSystem;
            } else {
                GUILayout.BeginHorizontal();
                GUILayout.Label("Toggle L-system type");
                if (GUILayout.Button($"{LSystem.Type}")) {
                    LSystem.Type = LSystem.Type == LSystemConfiguration.ConfigurationType.LSystem ? LSystemConfiguration.ConfigurationType.Alias : LSystemConfiguration.ConfigurationType.LSystem;
                }
                GUILayout.EndHorizontal();
            }

            switch (LSystem.Type) {
                case LSystemConfiguration.ConfigurationType.LSystem:
                    LSystemGUI();
                    break;
                case LSystemConfiguration.ConfigurationType.Alias:
                    AliasGUI();
                    break;
            }

            CustomInspectorTools.EndArea();
        }

        private void LSystemGUI() {
            LSystem.Axiom = CustomInspectorTools.TextArea("Axiom", LSystem.Axiom, int.MaxValue, true);
            LSystem.Iterations = CustomInspectorTools.PositiveIntegerSliderWithArbitraryChoice("Iterations", "Too many iterations may cause extemely slow generation times.", ref iterationToggle, LSystem.Iterations, 15);

            LSystem.UseSeed = CustomInspectorTools.BoolField("Use seed", LSystem.UseSeed);

            if (LSystem.UseSeed) {
                LSystem.Seed = CustomInspectorTools.IntegerSlider("Seed", LSystem.Seed, 0, int.MaxValue - 10);
            }

            LSystem.CaseSensitive = CustomInspectorTools.BoolField("Case sensitive", LSystem.CaseSensitive);
        }


        private void AliasGUI() {
            LSystem.Axiom = CustomInspectorTools.TextArea("Alias", LSystem.Axiom, int.MaxValue, true);
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
            GrammarRules();
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Add character definition")) {
                LSystem.CharacterDefinitions.Add(new LSystemCharacterSetting());
            }
            if (GUILayout.Button("Clear definitions")) {
                LSystem.CharacterDefinitions.Clear();
            }

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

            GUILayout.BeginHorizontal();
            GUILayout.Label("Toggle rule type");
            if (GUILayout.Button(rule.IsAlias ? "Alias" : "Iteration command")) {
                rule.IsAlias = !rule.IsAlias;
            }
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
            ProbabilityRule rule = charDef.Rules[index];

            string rString;
            if (detailedRuleEditMode == 2) {
                GUILayout.ExpandWidth(false);
                rString = EditorGUILayout.TextField(rule.Rule);

                if (GUILayout.Button("Remove")) {
                    charDef.Rules.RemoveAt(index);
                }

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
    }
}
