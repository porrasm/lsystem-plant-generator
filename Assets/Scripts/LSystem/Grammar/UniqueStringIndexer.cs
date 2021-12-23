using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Default {
    public class UniqueStringIndexer : IEnumerable<KeyValuePair<string, int>> {
        #region fields
        public int UniqueCount { get; private set; }
        private Dictionary<string, int> rules = new Dictionary<string, int>();
        #endregion

        public int SetAndGetIndex(string s) {
            if (rules.ContainsKey(s)) {
                return rules[s];
            }
            rules.Add(s, UniqueCount);
            return UniqueCount++;
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        public IEnumerator<KeyValuePair<string, int>> GetEnumerator() => rules.GetEnumerator();
    }
}
