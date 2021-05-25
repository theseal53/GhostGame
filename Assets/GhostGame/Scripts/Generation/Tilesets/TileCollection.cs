using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public enum TilePosition
{
    TopLeft, TopCenter, TopRight,
    MidLeft, MidCenter, MidRight,
    BottomLeft, BottomCenter, BottomRight,
    VerticalHall, HorizontalHall,
    Empty, Wall
}

public enum TileType
{
    Wall, Floor
}

public class TileCollection : MonoBehaviour
{
    public Tile[] topLeftTiles;
    public Tile[] topCenterTiles;
    public Tile[] topRightTiles;
    public Tile[] midLeftTiles;
    public Tile[] midCenterTiles;
    public Tile[] midRightTiles;
    public Tile[] bottomLeftTiles;
    public Tile[] bottomCenterTiles;
    public Tile[] bottomRightTiles;
    public Tile[] verticalHallTiles;
    public Tile[] horizontalHallTiles;

    public Tile[] getPositionalSet(TilePosition tilePosition)
	{
        Tile[] returnArray;
        switch(tilePosition)
		{
            case TilePosition.TopLeft:
                returnArray = topLeftTiles;
                break;
            case TilePosition.TopCenter:
                returnArray = topCenterTiles;
                break;
            case TilePosition.TopRight:
                returnArray = topRightTiles;
                break;
            case TilePosition.MidLeft:
                returnArray = midLeftTiles;
                break;
            case TilePosition.MidCenter:
                returnArray = midCenterTiles;
                break;
            case TilePosition.MidRight:
                returnArray = midRightTiles;
                break;
            case TilePosition.BottomLeft:
                returnArray = bottomLeftTiles;
                break;
            case TilePosition.BottomCenter:
                returnArray = bottomCenterTiles;
                break;
            case TilePosition.BottomRight:
                returnArray = bottomRightTiles;
                break;
            case TilePosition.VerticalHall:
                returnArray = verticalHallTiles;
                break;
            case TilePosition.HorizontalHall:
                returnArray = horizontalHallTiles;
                break;
            default:
                returnArray = midCenterTiles;
                break;
        }
        if (returnArray.Length == 0)
            return midCenterTiles;
        else
            return returnArray;
    }
}
