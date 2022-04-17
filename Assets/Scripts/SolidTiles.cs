using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System;

public class SolidTiles : MonoBehaviour {

    public Tile[] walkableTiles;
    public Tile[] shipMovableTiles;
    public Tile[] shipDockingTiles;
    public Tile[] canoeMovableTiles;
    public Tile[] airshipLandableTiles;

    public bool isWalkableTile(Tile tile) {
        return Array.Exists<Tile>(walkableTiles, currentWalkableTile => tile == currentWalkableTile);
    }

    public bool isShipMovableTile(Tile tile) {
        return Array.Exists<Tile>(shipMovableTiles, currentShipMovableTile => tile == currentShipMovableTile);
    }

    public bool isShipDockingTile(Tile tile) {
        return Array.Exists<Tile>(shipDockingTiles, currentShipDockingTile => tile == currentShipDockingTile);
    }

    public bool isCanoeMovableTile(Tile tile) {
        return Array.Exists<Tile>(canoeMovableTiles, currentCanoeWalkableTile => tile == currentCanoeWalkableTile);
    }

    public bool isAirshipLandable(Tile tile) {
        return Array.Exists<Tile>(airshipLandableTiles, currentAirshipLandableTile => tile == currentAirshipLandableTile);
    }
}
