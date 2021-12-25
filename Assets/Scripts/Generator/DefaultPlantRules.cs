using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Default {
    public static class DefaultPlantRules {
        private const float angle = 25;

        public class Forward : ICharacterRule {
            public string Character => "f";
            public string Description => "Draw a line forward with some length and width";

            public void Apply(PlantDensityMap plant, PlantBranching<TurtleState> branching, ref TurtleState state) {
                Vector3 end = state.Position + (state.Forward * state.LineLength);
                plant.DrawLine(state.Position, end, state.LineWidth);
                Debug.DrawLine(state.Position, end);
                state.Position = end;
            }
        }
        public class Branch : ICharacterRule {
            public string Character => "(";
            public string Description => "Create a branch";

            public void Apply(PlantDensityMap plant, PlantBranching<TurtleState> branching, ref TurtleState state) {
                branching.Branch();
            }
        }
        public class Debranch : ICharacterRule {
            public string Character => ")";
            public string Description => "Return from branch";

            public void Apply(PlantDensityMap plant, PlantBranching<TurtleState> branching, ref TurtleState state) {
                state = branching.Debranch();
            }
        }
        public class XAngleAdd : ICharacterRule {
            public string Character => "x+";
            public string Description => "Add X angle";

            public void Apply(PlantDensityMap plant, PlantBranching<TurtleState> branching, ref TurtleState state) {
                state.Forward = state.Forward.Rotate(angle, Vector3.right);
            }
        }
        public class XAngleRemove : ICharacterRule {
            public string Character => "x-";
            public string Description => "Remove X angle";

            public void Apply(PlantDensityMap plant, PlantBranching<TurtleState> branching, ref TurtleState state) {
                state.Forward = state.Forward.Rotate(-angle, Vector3.right);
            }
        }
        public class YAngleAdd : ICharacterRule {
            public string Character => "y+";
            public string Description => "Add Y angle";

            public void Apply(PlantDensityMap plant, PlantBranching<TurtleState> branching, ref TurtleState state) {
                state.Forward = state.Forward.Rotate(angle, Vector3.up);
            }
        }
        public class YAngleRemove : ICharacterRule {
            public string Character => "y-";
            public string Description => "Remove Y angle";

            public void Apply(PlantDensityMap plant, PlantBranching<TurtleState> branching, ref TurtleState state) {
                state.Forward = state.Forward.Rotate(-angle, Vector3.up);
            }
        }
    }
}
