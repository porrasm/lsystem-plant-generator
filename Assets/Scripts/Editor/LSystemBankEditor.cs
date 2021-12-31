using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Default {
    [CustomEditor(typeof(LSystemBank))]
    public class LSystemBankEditor : Editor {
        #region fields
        private LSystemBank targetBank;
        private LSystemBank Bank {
            get {
                if (targetBank == null) {
                    targetBank = (LSystemBank)target;
                }
                return targetBank;
            }
        }

        private bool[] folds;
        #endregion

        override public void OnInspectorGUI() {
            Validate();
            for (int i = 0; i < Bank.LSystems.Count; i++) {
                LSystemEditGUI(Bank.LSystems[i], i);
            }

            if (GUILayout.Button("Add L-system")) {
                Bank.LSystems.Add(new LSystemConfiguration());
            }
        }

        private void Validate() {
            if (Bank.LSystems == null) {
                Logger.Log("Reset LSystems");
                Bank.LSystems = new List<LSystemConfiguration>();
            }
            for (int i = 0; i < Bank.LSystems.Count; i++) {
                if (Bank.LSystems[i] == null) {
                    Bank.LSystems[i] = new LSystemConfiguration();
                }
            }

            if (folds == null || folds.Length != Bank.LSystems.Count) { 
                folds = new bool[Bank.LSystems.Count];
            }
        }

        private void LSystemEditGUI(LSystemConfiguration lsystem, int index) {
            CustomInspectorTools.CreateBox();
            GUILayout.BeginHorizontal();
            GUILayout.Label("L-system name: " + (lsystem.LSystemName.Length == 0 ? "Untitled" : lsystem.LSystemName));

            if (GUILayout.Button("Remove")) {
                Bank.LSystems.RemoveAt(index);
            }

            GUILayout.EndHorizontal();

            bool fold = folds[index];
            CustomInspectorTools.CreateFoldedArea("Edit L-system", ref fold);
            if (fold) {
                LSystemConfigurationGUI gui = new LSystemConfigurationGUI(lsystem, false);
                gui.CreateGUI();
            }
            folds[index] = fold;
            CustomInspectorTools.EndArea();
        }
    }
}
