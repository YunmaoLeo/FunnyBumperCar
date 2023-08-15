
    using Unity.Mathematics;
    using Unity.VisualScripting;

    public class LocalRandom
    {
        private static LocalRandom instance = null;
        public static LocalRandom Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new LocalRandom();
                }

                return instance;
            }
        }
        
        private LocalRandom()
        {
            random = new Random(1);
        }
        
        private Random random;

        public float GetRandomFloat(float min, float max)
        {
            return random.NextFloat(min, max);
        }
    }
