using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Default {
    public interface ICharacterRule {
        string Character { get; }
        string Description { get; }
        void Apply(PlantDensityMap plant, PlantBranching<TurtleState> branching, ref TurtleState state);
    }
}
