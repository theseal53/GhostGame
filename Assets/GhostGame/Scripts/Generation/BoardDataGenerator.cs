using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardDataGenerator
{
    public IntRange numRooms = new IntRange(10, 10);
    public IntRange corridorLength = new IntRange(3, 5);

    private int boardMargin = 1;
    private int roomMargin = 2;

    public Board board;

    // Start is called before the first frame update
    public Board GenerateBoardData(int stories, int columns, int rows)
    {
        board = new Board(stories, columns, rows);
        board.roomMargin = roomMargin;
        board.boardMargin = boardMargin;
        CreateRoomsAndCorridors();
        SetTilesValuesForRooms();
        SetTilesValuesForCorridors();
        return board;
    }

    private void CreateRoomsAndCorridors()
	{
        Basement basement = new Basement(Constants.BASEMENT);
        GroundStory groundStory = new GroundStory(Constants.GROUND_STORY);
        UpperStory upperStory = new UpperStory(Constants.UPPER_STORY);
        board.stories.Add(basement);
        board.stories.Add(groundStory);
        board.stories.Add(upperStory);

        groundStory.CreateRoomsAndCorridors(board);
        basement.CreateRoomsAndCorridors(board, groundStory.landing.stairwayDown, Verticality.Down);
        upperStory.CreateRoomsAndCorridors(board, groundStory.landing.stairwayUp, Verticality.Up);
	}

    /*private Room RandomRoom()
	{
        int rng = roomRngIdentifiers[0];
		switch (rng)
		{
            case 0:
                return new Library();
            case 1:
                return new DiningRoom();
            case 2:
                return new Bathroom();
            case 3:
                return new Kitchen();
            default:
                return new Room();
		}
	}*/

    private void SetTilesValuesForRooms()
	{
        foreach (Story story in board.stories)
        {
            story.SetTilesValuesForRooms(board);
        }
    }

    private void SetTilesValuesForCorridors()
	{
        foreach (Story story in board.stories)
        {
            story.SetTilesValuesForCorridors(board);
        }
    }

}
