using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using UnityEngine;

namespace Default {

    public static class Parser {
        public static float ParseFloat(string s) {
            return float.Parse(s, NumberStyles.Any, CultureInfo.InvariantCulture);
        }

        public static Color ParseColor(string s) {
            s = s.Replace("#", "0x");
            int v = Convert.ToInt32(s, 16);
            int r = (v >> 16) & 255;
            int g = (v >> 8) & 255;
            int b = v & 255;
            return new Color32((byte)r, (byte)g, (byte)b, 255);
        }

        public static Vector3 ParseVector3(string s) {
            try {
                float v = ParseFloat(s);
                return new Vector3(v, v, v);
            } catch (Exception) {
                if (s.StartsWith("(") && s.EndsWith(")")) {
                    s = s.Substring(1, s.Length - 2);
                }

                // split the items
                string[] sArray = s.Split(',');

                // store as a Vector3
                Vector3 result = new Vector3(
                    ParseFloat(sArray[0]),
                    ParseFloat(sArray[1]),
                    ParseFloat(sArray[2]));

                return result;
            }
        }

        public static List<string> ParseWords(string plant, char splitChar, char joinedWordStart, char joinedWordEnd) {
            List<string> result = new List<string>();
            StringBuilder sb = new StringBuilder();

            void AddWord(bool isCommand) {
                if (sb.Length > 0) {
                    string cmd = isCommand ? $"{joinedWordStart}{sb.ToString()}{joinedWordEnd}" : sb.ToString();
                    result.Add(cmd);
                    sb.Clear();
                }
            }

            bool commandMode = false;

            foreach (char c in plant) {
                if (c == splitChar && !commandMode) {
                    AddWord(false);
                    continue;
                }
                if (c == joinedWordStart && !commandMode) {
                    commandMode = true;
                    AddWord(false);
                    continue;
                }
                if (c == joinedWordEnd && commandMode) {
                    commandMode = false;
                    AddWord(true);
                    continue;
                }

                sb.Append(c);
            }
            AddWord(false);

            return result;
        }
    }
}
