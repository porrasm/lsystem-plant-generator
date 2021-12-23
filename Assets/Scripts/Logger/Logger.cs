using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Default {
    public class Logger : MonoBehaviour {

        #region
        [SerializeField]
        private List<LogCategory> enabledCategories = new List<LogCategory>();
        public List<LogCategory> EnabledCategories { get => enabledCategories; }

        private static HashSet<LogCategory> enabledGlobal;
        private static bool setByLogger;
        #endregion

        private void Awake() {
            if (setByLogger) {
                return;
            }
            enabledGlobal.Clear();
            foreach (LogCategory v in enabledCategories) {
                enabledGlobal.Add(v);
            }
            setByLogger = true;

            List<LogCategory> notEnabled = new List<LogCategory>();

            string warning = "Some logger categories are disabled";

            foreach (LogCategory cat in Categories) {
                if (!enabledGlobal.Contains(cat)) {
                    notEnabled.Add(cat);
                    warning += "\nNot enabled: " + cat;
                }
            }

            if (notEnabled.Count > 0) {
                Debug.LogWarning(warning);
            }
        }

        static Logger() {
            //Debug.Log("Static logger");
            enabledGlobal = new HashSet<LogCategory>();
            foreach (LogCategory v in Enum.GetValues(typeof(LogCategory))) {
                enabledGlobal.Add(v);
            }
        }

        public static void Log(object message, LogCategory cat = LogCategory.Ungategorized) {
            if (!enabledGlobal.Contains(cat)) {
                return;
            }
            string msg = ObjectToString(message);

            Debug.Log(msg);
        }

        public static void LogObjects(string delimeter, params object[] objects) {
            string msg = ObjectsToString(delimeter, objects);

            Debug.Log(msg);
        }

        public static void LogFormat(string format, params object[] args) {

            Debug.LogFormat(format, args);
        }

        public static void Reminder(object message) {
            string msg = ObjectToString(message);
            Debug.LogWarning("<color=orange>Reminder:</color> " + msg);
        }

        public static void Warning(object message, LogCategory cat = LogCategory.Ungategorized) {
            if (!enabledGlobal.Contains(cat)) {
                return;
            }
            string msg = ObjectToString(message);

            Debug.LogWarning(msg);
        }

        public static void WarningObjects(string delimeter, params object[] objects) {
            string msg = ObjectsToString(delimeter, objects);
            Debug.LogWarning(msg);
        }

        public static void WarningFormat(string format, params object[] args) {
            Debug.LogWarningFormat(format, args);
        }

        public static void Error(object message, LogCategory cat = LogCategory.Ungategorized) {
            if (!enabledGlobal.Contains(cat)) {
                return;
            }
            string msg = ObjectToString(message);

            Debug.LogError(msg);
        }

        public static void ErrorObjects(string delimeter, params object[] objects) {
            string msg = ObjectsToString(delimeter, objects);

            Debug.LogError(msg);
        }

        public static void ErrorFormat(string format, params object[] args) {
            Debug.LogErrorFormat(format, args);
        }

        /// <summary>
        /// Lists the following variables as prints
        /// </summary>
        /// <param name="vars">Array with even amount of objects. Odd number is for variable name, even values are for the actual variable values</param>
        public static void LogVariables(params object[] vars) {
            if (vars.Length % 2 != 0) {
                throw new Exception("Var count is not divisible by 2");
            }

            for (int i = 0; i < vars.Length; i += 2) {
                string msg = VarString(vars[i], vars[i + 1]);
                Debug.Log(msg);
            }
        }

        private static string ObjectToString(object o) {
            return o?.ToString() ?? "null";
        }

        private static String ObjectsToString(string delimiter, params object[] objects) {
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < objects.Length - 1; i++) {
                sb.Append(ObjectToString(objects[i])).Append(delimiter);
            }
            sb.Append(ObjectToString(objects[objects.Length - 1]));

            return sb.ToString();
        }

        private static string VarString(object o) {
            return VarString(nameof(o), o);
        }

        private static string VarString(object n, object o) {
            return n + ": " + (o?.ToString() ?? "null");
        }

        public static Array Categories {
            get {
                return Enum.GetValues(typeof(LogCategory));
            }
        }
    }
}