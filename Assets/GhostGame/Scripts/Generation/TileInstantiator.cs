using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileInstantiator
{
    public void InstantiateTiles(Board board)
    {

        for (int i = 0; i < board.tilePositions.Length; ++i)
        {
            for (int j = 0; j < board.tilePositions[i].Length; j++)
            {
                if (board.tilePositions[i][j] == TilePosition.Wall)
				{
                    InstantiateWall(j, i);
				}
                else if (board.tilePositions[i][j] != TilePosition.Empty)
                {
                    Room room = board.rooms[board.tiles[i][j]];
                    TileSet tileSet = room.TileSet;
                    Tile[] positionalSet = tileSet.getPositionalSet(board.tilePositions[i][j]);
                    InstantiateFromArray(positionalSet, j, i);
                }
            }
        }
    }

    void InstantiateWall(int xCoord, int yCoord)
	{
        Vector3Int position = new Vector3Int(xCoord, yCoord, 0);
        TileSetRegistry.I.wallTilemap.SetTile(position, TileSetRegistry.I.wall);
    }

    void InstantiateFromArray(Tile[] prefabs, int xCoord, int yCoord)
    {
        int randomIndex = UnityEngine.Random.Range(0, prefabs.Length);

        Vector3Int position = new Vector3Int(xCoord, yCoord, 0);
        TileSetRegistry.I.floorTilemap.SetTile(position, prefabs[randomIndex]);
    }
}
