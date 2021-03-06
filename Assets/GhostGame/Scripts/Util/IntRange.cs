﻿//Taken largely from https://www.youtube.com/watch?v=wnoLaui3uO4

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SerializeField]
public class IntRange
{

    public int m_Min;
    public int m_Max;
    public IntRange(int min, int max)
	{
        m_Min = min;
        m_Max = max;
	}


    public int Random
	{
        get { return UnityEngine.Random.Range(m_Min, m_Max + 1);  }
	}
}
