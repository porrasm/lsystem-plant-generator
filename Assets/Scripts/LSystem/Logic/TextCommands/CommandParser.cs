using MiscUtil;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Default.CommandParser {
    public class TextCommandParser {
        #region fields
        private OperationSet<float> opFloat;
        private OperationSet<Color> opColor;

        private Dictionary<string, ValueType> variables;

        private class ValueType {
            public Type Type;
            public object Value;

            public ValueType(Type type, object value) {
                Type = type;
                Value = value;
            }
        }
        #endregion

        public TextCommandParser(PlantGeneratorSettings3D settings) {
            opFloat = new OperationSet<float>(float.Parse, (a, b) => a % b);
            opColor = new OperationSet<Color>(Matht.ParseColor, null);

            variables = GetValues(settings);
        }

        public void ApplyCommand(string command) {
            Command cmd = new Command(command);
            ValueType type = variables[cmd.Variable];

            if (typeof(float) == type.Type) {
                ApplyOperation(cmd, opFloat);
            } else if (typeof(Color) == type.Type) {
                ApplyOperation(cmd, opColor);
            }
        }

        private void ApplyOperation<T>(Command cmd, OperationSet<T> operations) {
            variables[cmd.Variable].Value = operations.GetOperation(cmd.Operation)((T)variables[cmd.Variable].Value, operations.Parser(cmd.Value));
        }

        #region target data
        private Dictionary<string, ValueType> GetValues(PlantGeneratorSettings3D settings) {
            Dictionary<string, ValueType> values = new Dictionary<string, ValueType>();

            values.Add("linelength", new ValueType(typeof(float), settings.LineLength));
            values.Add("linewidth", new ValueType(typeof(float), settings.LineWidth));
            values.Add("anglex", new ValueType(typeof(float), settings.AngleX));
            values.Add("angley", new ValueType(typeof(float), settings.AngleY));
            values.Add("color", new ValueType(typeof(Color), settings.Color));

            return values;
        }

        public PlantGeneratorSettings3D GetSettings() => new PlantGeneratorSettings3D() {
            LineLength = (float)variables["linelength"].Value,
            LineWidth = (float)variables["linewidth"].Value,
            AngleX = (float)variables["anglex"].Value,
            AngleY = (float)variables["angley"].Value,
            Color = (Color)variables["color"].Value,
        };
        #endregion
    }
}
