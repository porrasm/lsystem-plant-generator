using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Default {
    public static class DefaultPlantRules {
        private const float angle = 25;
        private static Transform reft;
        private static Transform Reft {
            get {
                if (reft == null) {
                    reft = GameObject.FindGameObjectWithTag("Player").transform;
                }
                return reft;
            }
        }

        public class Forward : ICharacterRule {
            public string Character => "f";
            public string Description => "Draw a line forward with some length and width";

            public void Apply(PlantDensityMap plant, PlantBranching<TurtleState> branching, ref TurtleState state) {
                Reft.eulerAngles = state.EulerAngles;
                Vector3 end = state.Position + (Reft.forward * state.LineLength);
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
                Vector3 pos = state.EulerAngles;
                pos.x = (pos.x + angle) % 360;
                state.EulerAngles = pos;
            }
        }
        public class XAngleRemove : ICharacterRule {
            public string Character => "x-";
            public string Description => "Remove X angle";

            public void Apply(PlantDensityMap plant, PlantBranching<TurtleState> branching, ref TurtleState state) {
                Vector3 pos = state.EulerAngles;
                pos.x = (pos.x - angle) % 360;
                state.EulerAngles = pos;
            }
        }
        public class YAngleAdd : ICharacterRule {
            public string Character => "y+";
            public string Description => "Add Y angle";

            public void Apply(PlantDensityMap plant, PlantBranching<TurtleState> branching, ref TurtleState state) {
                Vector3 pos = state.EulerAngles;
                pos.y = (pos.y + angle) % 360;
                state.EulerAngles = pos;
            }
        }
        public class YAngleRemove : ICharacterRule {
            public string Character => "y-";
            public string Description => "Remove Y angle";

            public void Apply(PlantDensityMap plant, PlantBranching<TurtleState> branching, ref TurtleState state) {
                Vector3 pos = state.EulerAngles;
                pos.y = (pos.y - angle) % 360;
                state.EulerAngles = pos;
            }
        }
    }
}
