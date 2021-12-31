using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Default {
    public struct Range {
        #region fields
        public int Min, Max;
        #endregion

        public Range(int min, int max) {
            Min = min;
            Max = max;
        }

        public void Extend(int val) {
            if (IsValid()) {
                Min = Mathf.Min(Min, val);
                Max = Mathf.Max(Max, val);
            } else {
                Min = val;
                Max = val;
            }
        }

        public bool IsValid() => Min <= Max;
        public bool Contains(float val) => val >= Min && val <= Max;
    }
}
