using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ArrayPrinter
{
    public static void Print3DArray(sbyte[][][] matrix, string path)
    {
        File.WriteAllText(path, string.Empty);
        Debug.Log("Printing array to " + path);
        StreamWriter writer = new StreamWriter(path);

        for (int i = 0; i < matrix.Length; i++)
        {
            for (int j = 0; j < matrix[0].Length; j++)
            {
                for (int k = 0; k < matrix[0][0].Length; k++)
                {
                    if (matrix[i][j][k] == -1)
                        writer.Write("X ");
                    else
                        writer.Write(matrix[i][j][k] + " ");
                }
                writer.WriteLine();
            }
            writer.WriteLine();
        }
    }
    public static T[] To1DArray<T>(T[,] input)
    {
        // Step 1: get total size of 2D array, and allocate 1D array.
        int size = input.Length;
        T[] result = new T[size];

        // Step 2: copy 2D array elements into a 1D array.
        int write = 0;
        for (int i = 0; i <= input.GetUpperBound(0); i++)
        {
            for (int z = 0; z <= input.GetUpperBound(1); z++)
            {
                result[write++] = input[i, z];
            }
        }
        // Step 3: return the new array.
        return result;
    }

    public static T[,] To2DArray<T>(T[] input, int rows, int columns)
    {
        // Step 1: get total size of 2D array, and allocate 1D array.
        T[,] result = new T[rows, columns];

        // Step 2: copy 2D array elements into a 1D array.
        int write = 0;
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                result[i, j] = input[write];
                write++;
            }
        }
        // Step 3: return the new array.
        return result;
    }
}
