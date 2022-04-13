using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System;

public class SolidTiles : MonoBehaviour {

    public Tile[] walkableTiles;
    public Tile[] shipMovableTiles;
    public Tile[] shipDockingTiles;
    public Tile[] canoeWalkableTiles;

    public bool isWalkableTile(Tile tile) {
        return Array.Exists<Tile>(walkableTiles, currentWalkableTile => tile == currentWalkableTile);
    }

    public bool isShipMovableTile(Tile tile) {
        return Array.Exists<Tile>(shipMovableTiles, currentShipMovableTile => tile == currentShipMovableTile);
    }

    public bool isShipDockingTile(Tile tile) {
        return Array.Exists<Tile>(shipDockingTiles, currentShipDockingTile => tile == currentShipDockingTile);
    }

    public bool isCanoeWalkableTile(Tile tile) {
        return Array.Exists<Tile>(canoeWalkableTiles, currentCanoeWalkableTile => tile == currentCanoeWalkableTile);
    }
}
