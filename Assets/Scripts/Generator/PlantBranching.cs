using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Default {
    public struct PlantBranching<T> {
        public Action Branch { get; private set; }
        public Action<T> BranchState { get; private set; }
        public Func<T> Debranch { get; private set; }

        public PlantBranching(Action branch, Action<T> branchState, Func<T> debranch) {
            Branch = branch;
            BranchState = branchState;
            Debranch = debranch;
        }
    }
}
