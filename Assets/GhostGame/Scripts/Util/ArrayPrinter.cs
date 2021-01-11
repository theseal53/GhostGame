using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ArrayPrinter
{
    public static void Print2DArray(int[][] matrix, string path)
    {
        File.WriteAllText(path, string.Empty);
        Debug.Log("Printing array to " + path);
        StreamWriter writer = new StreamWriter(path);
        for (int i = 0; i < matrix.Length; i++)
        {
            for (int j = 0; j < matrix[0].Length; j++)
            {
                writer.Write(matrix[i][j] + " ");
            }
            writer.WriteLine();
        }
    }
}
