namespace Default {
    public class Xorshift128 {

        #region fields
        private const int A = 15;
        private const int B = 21;
        private const int C = 4;

        private int x, y, z, w;
        #endregion

        #region initialization
        public Xorshift128(int seed) {
            SetSeed(seed);
        }
        public Xorshift128(int x, int y, int z, int w) {
            SetSeeds(x, y, z, w);
        }
        public void SetSeed(int seed) {
            SetSeeds(seed, seed, seed, seed);
        }
        public void SetSeeds(int x, int y, int z, int w) {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = w;
        }

        public int[] GetState() {
            return new int[] { x, y, z, w };
        }
        public void SetState(int[] state) {
            x = state[0];
            y = state[1];
            z = state[2];
            w = state[3];
        }
        #endregion

        public int Next {
            get {
                int tmp = x ^ (x << A);
                x = y;
                y = z;
                z = w;
                w = w ^ (w >> B) ^ tmp ^ (tmp >> C);
                return w;
            }
        }
    }
}