using System.Collections;
using System.Collections.Generic;
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
        #endregion

        public override void OnInspectorGUI() {
            GUI.CreateGUI();
            if (LSystem.GetComponent<LSystemBank>() == null && GUILayout.Button("Add sub-L-systems")) {
                LSystem.gameObject.AddComponent<LSystemBank>();
            }
        }
    }
}
