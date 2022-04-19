using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public abstract class TileMovementData : MonoBehaviour {
    public abstract bool isWalkableTile(Tile tile);
    public abstract bool isShipMovableTile(Tile tile);
    public abstract bool isShipDockingTile(Tile tile);
    public abstract bool isCanoeMovableTile(Tile tile);
    public abstract bool isAirshipLandable(Tile tile);
}
