using MiscUtil;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Default.CommandParser {
    public class OperationSet<T> {
        #region fields
        public delegate T Operation(T a, T b);

        public Func<string, T> Parser { get; private set; }

        private Dictionary<string, Operation> operations;
        #endregion

        public OperationSet(Func<string, T> parser, Operation modulus) {
            this.Parser = parser;

            operations = new Dictionary<string, Operation>();

            operations.Add("=", (a, b) => (T)b);
            operations.Add("+=", (a, b) => Operator.Add((T)a, (T)b));
            operations.Add("-=", (a, b) => Operator.Subtract((T)a, (T)b));
            operations.Add("*=", (a, b) => Operator.Multiply((T)a, (T)b));
            operations.Add("/=", (a, b) => Operator.Divide((T)a, (T)b));
            operations.Add("%=", modulus);
        }

        public Operation GetOperation(string operation) => operations[operation];

        public override bool Equals(object obj) {
            return obj is OperationSet<T> operations;
        }

        public override int GetHashCode() {
            return typeof(T).GetHashCode();
        }
    }
}
