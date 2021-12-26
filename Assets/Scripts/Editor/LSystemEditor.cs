using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Default {
    [CustomEditor(typeof(LSystemConfigurer)), CanEditMultipleObjects]
    public class PlantSettingsEditor : Editor {

        #region fields
        private LSystemConfigurer lsystem;
        public LSystemConfigurer LSystem {
            get {
                if (lsystem == null) {
                    lsystem = (LSystemConfigurer)target;
                }
                return lsystem;
            }
        }

        private LSystemConfigurationGUI guiValue;
        private LSystemConfigurationGUI GUI {
            get {
                if (guiValue == null) {
                    guiValue = new LSystemConfigurationGUI(LSystem.PrimaryLSystem, true);
                }
                return guiValue;
            }
        }

        private bool bankFold;
        #endregion

        public override void OnInspectorGUI() {
            GUI.CreateGUI();
            CustomInspectorTools.CreateFoldedArea("External L-system references", ref bankFold);
            if (bankFold) {
                LSystemBankGUI();
            }
            CustomInspectorTools.EndFoldedArea();
        }

        private void LSystemBankGUI() {
            EditorGUILayout.HelpBox("External L-system references can be used to reference other L-systems from another L-system. For example you can create a separate L-system with the name \"branch\" and reference it from your primary L-system. Then you can use the text command \"$branch\" (separated with spaces) from your primary L-system and the \"branch\" L-system will be built within that section of the output string. \n\n It is also possible to recursively generate the primary L-system. The primary L-system is always named \"self\" so the keyword \"$self\" is always valid.", MessageType.Info);

            InitList(out LSystemBank selfBank);

            for (int i = 0; i < LSystem.SubSystems.Count; i++) {
                LSystemBank bank = LSystem.SubSystems[i];
                if (bank != null && bank.LSystems == null) {
                    bank.LSystems = new List<LSystemConfiguration>();
                }
                SubsystemGUI(bank != null && bank == selfBank, i);
            }
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Add L-system bank references")) {
                LSystem.SubSystems.Add(null);
            }
            if (selfBank == null && GUILayout.Button("Add L-system bank here")) {
                LSystem.SubSystems.Insert(0, LSystem.gameObject.AddComponent<LSystemBank>());
            }
            if (GUILayout.Button("Remove empty")) {
                LSystem.SubSystems = LSystem.SubSystems.Where(s => s != null).ToList();
            }
            GUILayout.EndHorizontal();
        }

        private void InitList(out LSystemBank bank) {
            if (LSystem.SubSystems == null) {
                LSystem.SubSystems = new List<LSystemBank>();
            }
            bank = LSystem.GetComponent<LSystemBank>();
            if (bank != null) {
                if (!LSystem.SubSystems.Contains(bank)) {
                    LSystem.SubSystems.Insert(0, bank);
                }
            }
        }

        private void SubsystemGUI(bool selfBank, int index) {
            CustomInspectorTools.CreateBox();
            LSystem.SubSystems[index] = (LSystemBank)EditorGUILayout.ObjectField($"Reference bank {index + 1}", LSystem.SubSystems[index], typeof(LSystemBank), true);
            LSystemBank bank = LSystem.SubSystems[index];

            if (bank != null) {
                GUILayout.Label($"L-system count: {bank.LSystems.Where(s => s != null).Count()}");
            }

            if (selfBank) {
                GUILayout.Label($"Delete the {nameof(LSystemBank)} component from this object to remove this reference");
            } else {
                if (GUILayout.Button("Remove reference")) {
                    LSystem.SubSystems.RemoveAt(index);
                }
            }
            CustomInspectorTools.EndArea();
        }
    }
}
