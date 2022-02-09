/*
 *       -3D-Engine-
 *     (c) Josh Bryant
 * https://joshdbryant.com
 *
 * Full license is available in the GitHub repository:
 * https://github.com/JoshB82/3D-Engine/blob/master/LICENSE
 *
 * Code description for this file:
 * Defines a two-dimensional buffer called Buffer2D that holds values of type T.
 */

using System;

namespace Imagenic.Core.Renderers;

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
        for (int i = 0; i < firstDimensionSize; i++)
        {
            for (int j = 0; j < secondDimensionSize; j++)
            {
                Values[i][j] = value;
            }
        }
    }

    public void SetAllToDefault() => SetAllToValue(default);

    public void ForEach(Action<T> action)
    {
        for (int i = 0; i < firstDimensionSize; i++)
        {
            for (int j = 0; j < secondDimensionSize; j++)
            {
                action(Values[i][j]);
            }
        }
    }

    public void ForEach(Action<T, int, int> action)
    {
        for (int i = 0; i < firstDimensionSize; i++)
        {
            for (int j = 0; j < secondDimensionSize; j++)
            {
                action(Values[i][j], i, j);
            }
        }
    }

    #endregion

    #region Casting

    public static explicit operator T[](Buffer2D<T> buffer)
    {
        T[] array = new T[buffer.firstDimensionSize * buffer.secondDimensionSize];
        buffer.ForEach((t, i, j) => array[i * buffer.firstDimensionSize + j] = buffer.Values[i][j]);
        return array;
    }

    #endregion
}