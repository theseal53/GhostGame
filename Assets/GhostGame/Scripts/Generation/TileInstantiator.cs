using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileInstantiator
{
    public void InstantiateTiles(short[,] tiles, TilePosition[,] tilePositions)
    {

        for (int i = 0; i < tilePositions.GetLength(0); ++i)
        {
            for (int j = 0; j < tilePositions.GetLength(1); j++)
            {
                if (tilePositions[i,j] == TilePosition.Wall)
				{
                    InstantiateWall(j, i);
				}
                else if (tilePositions[i,j] != TilePosition.Empty)
                {
                    //Room room = board.rooms[board.tiles[i][j]];
                    //TileSet tileSet = room.TileSet;
                    TileSet tileSet = TileSetRegistry.I.GetTileSet((RoomCode)tiles[i,j]);
                    Tile[] positionalSet = tileSet.getPositionalSet(tilePositions[i,j]);
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
