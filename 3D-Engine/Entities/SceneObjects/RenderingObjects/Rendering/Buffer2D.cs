namespace _3D_Engine.Entities.SceneObjects.RenderingObjects.Rendering
{
    public class Buffer2D<T>
    {
        #region Fields and Properties

        private int firstDimensionSize, secondDimensionSize;

        public int FirstDimensionSize
        {
            get => firstDimensionSize;
            set
            {
                firstDimensionSize = value;
                SetSizes(firstDimensionSize, secondDimensionSize);
            }
        }

        public int SecondDimensionSize
        {
            get => secondDimensionSize;
            set
            {
                secondDimensionSize = value;
                SetSizes(firstDimensionSize, secondDimensionSize);
            }
        }

        private void SetSizes(int firstDimensionSize, int secondDimensionSize)
        {
            Values = new T[firstDimensionSize][];
            for (int i = 0; i < firstDimensionSize; i++)
            {
                Values[i] = new T[secondDimensionSize];
            }
        }

        public T[][] Values { get; set; }

        #endregion

        #region Constructors

        public Buffer2D(int firstDimensionSize, int secondDimensionSize)
        {
            this.firstDimensionSize = firstDimensionSize;
            this.secondDimensionSize = secondDimensionSize;
            SetSizes(firstDimensionSize, secondDimensionSize);
        }

        #endregion

        #region Methods

        public void SetAllToValue(T value)
        {
            for (int i = 0; i < FirstDimensionSize; i++)
            {
                for (int j = 0; j < SecondDimensionSize; j++)
                {
                    Values[i][j] = value;
                }
            }
        }

        public void SetAllToDefault() => SetAllToValue(default);

        #endregion
    }
}