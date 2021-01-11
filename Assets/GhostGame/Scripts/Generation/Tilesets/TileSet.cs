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

public class TileSet : MonoBehaviour
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
        switch(tilePosition)
		{
            case TilePosition.TopLeft:
                return topLeftTiles;
            case TilePosition.TopCenter:
                return topCenterTiles;
            case TilePosition.TopRight:
                return topRightTiles;
            case TilePosition.MidLeft:
                return midLeftTiles;
            case TilePosition.MidCenter:
                return midCenterTiles;
            case TilePosition.MidRight:
                return midRightTiles;
            case TilePosition.BottomLeft:
                return bottomLeftTiles;
            case TilePosition.BottomCenter:
                return bottomCenterTiles;
            case TilePosition.BottomRight:
                return bottomRightTiles;
            case TilePosition.VerticalHall:
                return verticalHallTiles;
            case TilePosition.HorizontalHall:
                return horizontalHallTiles;
            default:
                return midCenterTiles;
        }
    }
}
