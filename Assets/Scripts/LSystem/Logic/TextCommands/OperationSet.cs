using MiscUtil;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Default.CommandParser {
    public class OperationSet<T> {
        #region fields
        public delegate T Operation(T a, T b);

        private Func<string, T> parser;
        public Func<T> Random { get; private set; }

        private Dictionary<string, Operation> operations;

        private const string RANDOM_VAL = "rnd";
        #endregion

        public OperationSet(Func<string, T> parser, Func<T> random, Operation modulus) {
            this.parser = parser;
            this.Random = random;

            operations = new Dictionary<string, Operation>();

            operations.Add("=", (a, b) => (T)b);
            operations.Add("+=", (a, b) => Operator.Add((T)a, (T)b));
            operations.Add("-=", (a, b) => Operator.Subtract((T)a, (T)b));
            operations.Add("*=", (a, b) => Operator.Multiply((T)a, (T)b));
            operations.Add("/=", (a, b) => Operator.Divide((T)a, (T)b));
            operations.Add("%=", modulus);
            operations.Add("min=", null);
            operations.Add("max=", null);
        }

        public void OverrideOperation(string op, Operation opF) {
            operations[op] = opF;
        }

        public Operation GetOperation(string operation) => operations[operation];

        public override bool Equals(object obj) {
            return obj is OperationSet<T> operations;
        }

        public T Parse(string input) {
            if (RANDOM_VAL.Equals(input)) {
                return Random();
            }
            return parser(input);
        }

        public override int GetHashCode() {
            return typeof(T).GetHashCode();
        }
    }
}
