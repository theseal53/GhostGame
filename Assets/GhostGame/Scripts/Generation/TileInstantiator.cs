using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileInstantiator
{
    public void InstantiateTiles(sbyte[][][] tiles, TilePosition[][][] tilePositions)
    {
        int stories = tilePositions.Length;
        int rows = tilePositions[0].Length;
        int columns = tilePositions[0][0].Length;

        for (int i = 0; i < stories; ++i)
        {
            for (int j = 0; j < rows; j++)
            {
                for (int k = 0; k < columns; ++k)
                {
                    if (tiles[i][j][k] == Constants.EMPTY_CODE)
					{
                        if (tilePositions[i][j][k] != TilePosition.MidCenter)
                        {
                            TileCollection tileCollection = TileSetRegistry.I.GetWallTileCollection(i);
                            Tile[] positionalSet = tileCollection.getPositionalSet(tilePositions[i][j][k]);
                            InstantiateWall(positionalSet, i, k, j);
                        }
                    }
                    else
					{
                        TileCollection tileCollection = TileSetRegistry.I.GetTileSet((RoomCode)tiles[i][j][k]).TileCollection;
                        Tile[] positionalSet = tileCollection.getPositionalSet(tilePositions[i][j][k]);
                        InstantiateFloor(positionalSet, i, k, j);
                    }
                }
            }
        }
    }

    void InstantiateWall(Tile[] prefabs, int story, int xCoord, int yCoord)
    {
        int randomIndex = Random.Range(0, prefabs.Length);

        Vector3Int position = new Vector3Int(xCoord, yCoord, 0);
        TileSetRegistry.I.wallTilemaps[story].SetTile(position, prefabs[randomIndex]);
    }

    void InstantiateFloor(Tile[] prefabs, int story, int xCoord, int yCoord)
    {
        int randomIndex = Random.Range(0, prefabs.Length);

        Vector3Int position = new Vector3Int(xCoord, yCoord, 0);
        TileSetRegistry.I.floorTilemaps[story].SetTile(position, prefabs[randomIndex]);
    }
}
