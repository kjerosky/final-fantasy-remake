using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System;

public class WorldMapTileMovementData : MonoBehaviour {

    private static string WORLD_TILES_NAME_PREFIX = "ff1-overworld-tiles_";

    public HashSet<string> walkableTileNames;
    public HashSet<string> shipMovableTileNames;
    public HashSet<string> shipDockingTileNames;
    public HashSet<string> canoeMovableTileNames;
    public HashSet<string> airshipLandableTileNames;

    void Awake() {
        shipMovableTileNames = new HashSet<string>();
        shipMovableTileNames.Add(WORLD_TILES_NAME_PREFIX + "7");
        shipMovableTileNames.Add(WORLD_TILES_NAME_PREFIX + "20");
        shipMovableTileNames.Add(WORLD_TILES_NAME_PREFIX + "21");
        shipMovableTileNames.Add(WORLD_TILES_NAME_PREFIX + "22");
        shipMovableTileNames.Add(WORLD_TILES_NAME_PREFIX + "35");

        shipDockingTileNames = new HashSet<string>();
        shipDockingTileNames.Add(WORLD_TILES_NAME_PREFIX + "81");
        shipDockingTileNames.Add(WORLD_TILES_NAME_PREFIX + "82");
        shipDockingTileNames.Add(WORLD_TILES_NAME_PREFIX + "83");
        shipDockingTileNames.Add(WORLD_TILES_NAME_PREFIX + "84");
        shipDockingTileNames.Add(WORLD_TILES_NAME_PREFIX + "94");
        shipDockingTileNames.Add(WORLD_TILES_NAME_PREFIX + "95");
        shipDockingTileNames.Add(WORLD_TILES_NAME_PREFIX + "96");

        canoeMovableTileNames = new HashSet<string>();
        canoeMovableTileNames.Add(WORLD_TILES_NAME_PREFIX + "42");
        canoeMovableTileNames.Add(WORLD_TILES_NAME_PREFIX + "43");
        canoeMovableTileNames.Add(WORLD_TILES_NAME_PREFIX + "55");
        canoeMovableTileNames.Add(WORLD_TILES_NAME_PREFIX + "56");
        canoeMovableTileNames.Add(WORLD_TILES_NAME_PREFIX + "70");
        canoeMovableTileNames.Add(WORLD_TILES_NAME_PREFIX + "72");

        airshipLandableTileNames = new HashSet<string>();
        airshipLandableTileNames.Add(WORLD_TILES_NAME_PREFIX + "46");
        airshipLandableTileNames.Add(WORLD_TILES_NAME_PREFIX + "47");
        airshipLandableTileNames.Add(WORLD_TILES_NAME_PREFIX + "59");
        airshipLandableTileNames.Add(WORLD_TILES_NAME_PREFIX + "60");
        airshipLandableTileNames.Add(WORLD_TILES_NAME_PREFIX + "68");
        airshipLandableTileNames.Add(WORLD_TILES_NAME_PREFIX + "73");
        airshipLandableTileNames.Add(WORLD_TILES_NAME_PREFIX + "101");
        airshipLandableTileNames.Add(WORLD_TILES_NAME_PREFIX + "104");
    }

    public bool isWalkableLand(Tile tile) {
        return !isShipMovableTile(tile) && !isCanoeMovableTile(tile);
    }

    public bool isShipMovableTile(Tile tile) {
        return shipMovableTileNames.Contains(tile.name);
    }

    public bool isShipDockingTile(Tile tile) {
        return shipDockingTileNames.Contains(tile.name);
    }

    public bool isCanoeMovableTile(Tile tile) {
        return canoeMovableTileNames.Contains(tile.name);
    }

    public bool isAirshipLandable(Tile tile) {
        return airshipLandableTileNames.Contains(tile.name);
    }
}
