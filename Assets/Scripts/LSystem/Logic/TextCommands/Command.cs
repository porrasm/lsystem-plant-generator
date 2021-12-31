using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Default.CommandParser {
    public class Command {
        public string Variable;
        public string Operation;
        public string Value;

        public Command(string cmdString) {
            string[] cmd = Parser.ParseWords(cmdString, ' ', '(', ')').ToArray();
            if (cmd.Length != 3) {
                throw new Exception("Invalid command statement count: " + cmd.Length);
            }
            foreach (string s in cmd) {
                if (s.Length == 0) {
                    throw new Exception("Invalid command. Statement length was 0");
                }
            }

            Variable = cmd[0].ToLower();
            Operation = cmd[1].ToLower();
            Value = cmd[2].ToLower();
        }
    }
}
