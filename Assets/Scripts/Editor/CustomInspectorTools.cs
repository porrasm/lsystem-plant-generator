using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Default {
    public static class CustomInspectorTools {

        #region areas
        public static void CreateArea(string area, int dividerWidth = 10, int spaceWidth = 0, bool boxed = true) {
            GUILayout.Space(dividerWidth);
            GUILayout.Label(area);
            GUILayout.Space(spaceWidth);
            if (boxed) {
                GUILayout.BeginVertical("box");
            }
        }
        public static void CreateBox() {
            GUILayout.BeginVertical("box");
        }
        public static void EndArea() {
            GUILayout.EndVertical();
        }

        public static bool CreateFoldedArea(string area, ref bool open, int dividerWidth = 0, int spaceWidth = 10) {

            open = EditorGUILayout.Foldout(open, area);

            if (open) {
                EditorGUI.indentLevel++;
                GUILayout.Space(dividerWidth);
            }

            return open;
        }
        public static void EndFoldedArea() {
            EditorGUI.indentLevel--;
        }
        #endregion

        #region integer
        public static int IntegerField(string label, int previousValue, int min = int.MinValue, int max = int.MaxValue) {
            GUILayout.BeginHorizontal();
            if (label.Length > 0) {
                GUILayout.Label(label);
            }
            int newValue = EditorGUILayout.IntField(previousValue);
            GUILayout.EndHorizontal();

            return Mathf.Clamp(newValue, min, max);
        }
        public static int IntegerSlider(string label, int previousValue, int min = int.MinValue, int max = int.MaxValue) {
            GUILayout.BeginHorizontal();
            if (label.Length > 0) {
                GUILayout.Label(label);
            }
            int newValue = EditorGUILayout.IntSlider(previousValue, min, max);
            GUILayout.EndHorizontal();

            return Mathf.Clamp(newValue, min, max);
        }
        public static int PositiveIntegerSliderWithArbitraryChoice(string label, string warning, ref bool toggle, int previousValue, int max = int.MaxValue) {
            GUILayout.BeginHorizontal();
            if (label.Length > 0) {
                GUILayout.Label(label);
            }
            int newValue;
            if (previousValue >= max || toggle) {
                toggle = true;
                newValue = EditorGUILayout.IntField(previousValue);
                GUILayout.EndHorizontal();
                if (warning.Length > 0) {
                    GUILayout.BeginHorizontal();
                    GUILayout.Label(warning);
                    if (GUILayout.Button("Reset")) {
                        newValue = 0;
                    }
                    GUILayout.EndHorizontal();
                }
            } else {
                newValue = EditorGUILayout.IntSlider(previousValue, 0, max);
                GUILayout.EndHorizontal();
            }

            if (newValue <= 0) {
                toggle = false;
            }

            return Mathf.Clamp(newValue, 0, int.MaxValue);
        }
        #endregion

        #region float
        public static float FloatField(string label, float previousValue, float min = float.MinValue, float max = float.MaxValue) {
            GUILayout.BeginHorizontal();
            if (label.Length > 0) {
                GUILayout.Label(label);
            }
            float newValue = EditorGUILayout.FloatField(previousValue);
            GUILayout.EndHorizontal();

            return Mathf.Clamp(newValue, min, max);
        }
        public static float FloatSlider(string label, float previousValue, float min = float.MinValue, float max = float.MaxValue) {
            GUILayout.BeginHorizontal();
            if (label.Length > 0) {
                GUILayout.Label(label);
            }
            float newValue = EditorGUILayout.Slider(previousValue, min, max);
            GUILayout.EndHorizontal();

            return Mathf.Clamp(newValue, min, max);
        }
        #endregion

        #region string
        public static string TextField(string label, string previousValue, int maxLength = int.MaxValue) {
            GUILayout.BeginHorizontal();
            if (label.Length > 0) {
                GUILayout.Label(label);
            }
            string newValue = GUILayout.TextField(previousValue);
            GUILayout.EndHorizontal();

            if (newValue == null) {
                return "";
            }
            if (newValue.Length > maxLength) {
                return newValue.Substring(0, maxLength);
            }

            return newValue;
        }
        public static string TextArea(string label, string previousValue, int maxLength = int.MaxValue, bool horizontal = false) {
            if (horizontal) {
                GUILayout.BeginHorizontal();
            }

            if (label.Length > 0) {
                GUILayout.Label(label);
            }
            string newValue = GUILayout.TextArea(previousValue);

            if (horizontal) {
                GUILayout.EndHorizontal();
            }

            if (newValue == null) {
                return "";
            }
            if (newValue.Length > maxLength) {
                return newValue.Substring(0, maxLength);
            }

            return newValue;
        }
        #endregion

        #region other types
        public static bool BoolField(string label, bool previousValue) {
            GUILayout.BeginHorizontal();
            if (label.Length > 0) {
                GUILayout.Label(label);
            }
            bool newValue = EditorGUILayout.Toggle(previousValue);
            GUILayout.EndHorizontal();

            return newValue;
        }

        public static Color ColorField(string label, Color previousValue) {
            GUILayout.BeginHorizontal();
            if (label.Length > 0) {
                GUILayout.Label(label);
            }
            Color newValue = EditorGUILayout.ColorField(previousValue);
            GUILayout.EndHorizontal();

            return newValue;
        }

        public static Vector2 Vector2Field(string label, Vector2 previousValue) {
            GUILayout.BeginHorizontal();

            Vector2 newVal = EditorGUILayout.Vector2Field(label, previousValue);

            GUILayout.EndHorizontal();

            return newVal;
        }
        #endregion

        #region wrappers
        public static AnimationCurve CurveField(string label, AnimationCurve curve) {
            GUILayout.BeginHorizontal();
            if (label.Length > 0) {
                GUILayout.Label(label);
            }
            AnimationCurve newVal = EditorGUILayout.CurveField(curve);
            GUILayout.EndHorizontal();

            return newVal;
        }
        #endregion
    }
}
