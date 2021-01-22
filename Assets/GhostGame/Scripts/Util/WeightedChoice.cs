using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public struct GameObjectDimensionSet
{
	public GameObject target;
	public int breadth;
	public GameObjectDimensionSet(GameObject target, int breadth)
	{
		this.target = target;
		this.breadth = breadth;
	}
}

public static class WeightedChoice
{
	public static float PrecompTotal(List<float> chances)
	{
		float total = 0;
		foreach (float value in chances)
		{
			total += value;
		}
		return total;
	}
    public static T Choose<T>(List<T> options, List<float> chances)
	{
		float total = PrecompTotal(chances);	
		return Choose(options, chances, total);
	}
	public static T Choose<T>(List<T> options, List<float> chances, float precompTotal)
	{
		float rng = Random.Range(0, precompTotal);

		float counter = 0;
		for (int i = 0; i < chances.Count; i++)
		{
			float value = chances[i];
			if (rng <= value + counter)
			{
				return options[i];
			}
			counter += value;
		}
		return options[0];
	}
}
