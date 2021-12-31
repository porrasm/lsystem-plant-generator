using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Default {
    public static class DefaultPlantRules {
        public class Forward : ICharacterRule {
            public string Character => "f";
            public string Description => "Draw a line forward with some length and width";

            public void Apply(PlantDensityMap plant, PlantBranching<TurtleState> branching, ref TurtleState state) {
                Vector3 end = state.Position + (state.Forward * state.LineLength);
                plant.DrawLine(state.Position, end, state.Color, state.LineWidth, state.Density);
                state.Position = end;
            }
        }
        public class Sphere : ICharacterRule {
            public string Character => "s";
            public string Description => "Draw a sphere at position with radius";

            public void Apply(PlantDensityMap plant, PlantBranching<TurtleState> branching, ref TurtleState state) {
                plant.DrawSphere(state.Position, state.LineWidth, state.Density, state.Color);
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
                state.Forward = state.Forward.Rotate(state.AngleX, Vector3.right);
            }
        }
        public class XAngleRemove : ICharacterRule {
            public string Character => "x-";
            public string Description => "Remove X angle";

            public void Apply(PlantDensityMap plant, PlantBranching<TurtleState> branching, ref TurtleState state) {
                state.Forward = state.Forward.Rotate(-state.AngleX, Vector3.right);
            }
        }
        public class YAngleAdd : ICharacterRule {
            public string Character => "y+";
            public string Description => "Add Y angle";

            public void Apply(PlantDensityMap plant, PlantBranching<TurtleState> branching, ref TurtleState state) {
                state.Forward = state.Forward.Rotate(state.AngleY, Vector3.forward);
            }
        }
        public class YAngleRemove : ICharacterRule {
            public string Character => "y-";
            public string Description => "Remove Y angle";

            public void Apply(PlantDensityMap plant, PlantBranching<TurtleState> branching, ref TurtleState state) {
                state.Forward = state.Forward.Rotate(-state.AngleY, Vector3.forward);
            }
        }
    }
}
