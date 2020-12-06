namespace _3D_Engine.Rendering
{
    public class Buffer2D<T>
    {
        public int FirstDimensionSize { get; set; }
        public int SecondDimensionSize { get; set; }

        public T[][] Values { get; set; }

        #region Constructors

        public Buffer2D(int firstDimensionSize, int secondDimensionSize)
        {
            FirstDimensionSize = firstDimensionSize;
            SecondDimensionSize = secondDimensionSize;

            Values = new T[firstDimensionSize][];
            for (int i = 0; i < firstDimensionSize; i++)
            {
                Values[i] = new T[secondDimensionSize];
            }
        }

        #endregion

        public void SetAllToDefault() => SetAllToValue(default);

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
    }
}