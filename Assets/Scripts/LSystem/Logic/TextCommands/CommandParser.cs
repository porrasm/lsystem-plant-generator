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
        private OperationSet<Vector3> opVector;

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

        public TextCommandParser(TurtleState settings) {
            opFloat = new OperationSet<float>(Parser.ParseFloat, () => RNG.Float, (a, b) => a % b);
            opColor = new OperationSet<Color>(Parser.ParseColor, () => new Color(RNG.Float, RNG.Float, RNG.Float), null);
            opVector = new OperationSet<Vector3>(Parser.ParseVector3, () => new Vector3(RNG.Float, RNG.Float, RNG.Float), null);

            opFloat.OverrideOperation("min=", Mathf.Min);
            opFloat.OverrideOperation("max=", Mathf.Max);


            opVector.OverrideOperation("*=", Matht.Multiply);
            opVector.OverrideOperation("/=", Matht.Divide);
            opVector.OverrideOperation("%=", Matht.Modulo);
            opVector.OverrideOperation("min=", Matht.Min);
            opVector.OverrideOperation("max=", Matht.Max);


            variables = GetValues(settings);
        }

        public void ApplyCommand(string command) {
            Command cmd = new Command(command);
            ValueType type = variables[cmd.Variable];

            if (typeof(float) == type.Type) {
                ApplyOperation(cmd, opFloat);
            } else if (typeof(Color) == type.Type) {
                ApplyOperation(cmd, opColor);
            } else if (typeof(Vector3) == type.Type) { 
                ApplyOperation(cmd, opVector);
            }
        }

        private void ApplyOperation<T>(Command cmd, OperationSet<T> operations) {
            variables[cmd.Variable].Value = operations.GetOperation(cmd.Operation)((T)variables[cmd.Variable].Value, operations.Parse(cmd.Value));
        }

        #region target data
        private Dictionary<string, ValueType> GetValues(TurtleState settings) {
            Dictionary<string, ValueType> values = new Dictionary<string, ValueType>();

            // use nameof
            values.Add("position", new ValueType(typeof(Vector3), settings.Position));
            values.Add("forward", new ValueType(typeof(Vector3), settings.Forward));
            values.Add("linelength", new ValueType(typeof(float), settings.LineLength));
            values.Add("linewidth", new ValueType(typeof(float), settings.LineWidth));
            values.Add("anglex", new ValueType(typeof(float), settings.AngleX));
            values.Add("angley", new ValueType(typeof(float), settings.AngleY));
            values.Add("density", new ValueType(typeof(float), settings.Density));
            values.Add("color", new ValueType(typeof(Color), settings.Color));

            return values;
        }

        public TurtleState GetSettings() => new TurtleState() {
            Position = (Vector3)variables["position"].Value,
            Forward = (Vector3)variables["forward"].Value,
            LineLength = (float)variables["linelength"].Value,
            LineWidth = (float)variables["linewidth"].Value,
            AngleX = (float)variables["anglex"].Value,
            AngleY = (float)variables["angley"].Value,
            Density = (float)variables["density"].Value,
            Color = (Color)variables["color"].Value,
        };
        #endregion
    }
}
