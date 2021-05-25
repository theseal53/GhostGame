using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PhysicsLayer
{
	LineOfSight = 8,
	Basement = 10,
	GroundStory = 11,
	UpperStory = 12
}

public static class Layering
{
	private static int BASEMENT_SORTING_ID = -1;
	private static int GROUNDSTORY_SORTING_ID = -1;
	private static int UPPERSTORY_SORTING_ID = -1;

	public static void InitializeSortingLayers()
	{
		BASEMENT_SORTING_ID = SortingLayer.NameToID("Basement");
		GROUNDSTORY_SORTING_ID = SortingLayer.NameToID("GroundStory");
		UPPERSTORY_SORTING_ID = SortingLayer.NameToID("UpperStory");
	}


	public static PhysicsLayer StoryToPhysicsLayer(int story)
	{
		if (story == Constants.BASEMENT)
			return PhysicsLayer.Basement;
		else if (story == Constants.UPPER_STORY)
			return PhysicsLayer.UpperStory;
		else
			return PhysicsLayer.GroundStory;
	}

	public static int StoryToSortingLayerID(int story)
	{
		if (BASEMENT_SORTING_ID == -1)
		{
			InitializeSortingLayers();
		}
		if (story == Constants.BASEMENT)
			return BASEMENT_SORTING_ID;
		else if (story == Constants.UPPER_STORY)
			return UPPERSTORY_SORTING_ID;
		else
			return GROUNDSTORY_SORTING_ID;
	}
}