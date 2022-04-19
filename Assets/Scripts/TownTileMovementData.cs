using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TownTileMovementData : TileMovementData {

    public HashSet<string> walkableTileNames;

    void Awake() {
        walkableTileNames = new HashSet<string>();
        walkableTileNames.Add("ff1-town-tiles_0");
        walkableTileNames.Add("ff1-town-tiles_1");
        walkableTileNames.Add("ff1-town-tiles_2");
        walkableTileNames.Add("ff1-town-tiles_13");
        walkableTileNames.Add("ff1-town-tiles_14");
        walkableTileNames.Add("ff1-town-tiles_15");
        walkableTileNames.Add("ff1-town-tiles_16");
        walkableTileNames.Add("ff1-town-tiles_17");
        walkableTileNames.Add("ff1-town-tiles_18");
        walkableTileNames.Add("ff1-town-tiles_19");
        walkableTileNames.Add("ff1-town-tiles_36");
        walkableTileNames.Add("ff1-town-tiles_37");
        walkableTileNames.Add("ff1-town-tiles_41");
        walkableTileNames.Add("ff1-town-tiles_42");
        walkableTileNames.Add("ff1-town-tiles_45");
        walkableTileNames.Add("ff1-town-tiles_48");
    }

    public override bool isWalkableTile(Tile tile) {
        return walkableTileNames.Contains(tile.name);
    }

    public override bool isShipMovableTile(Tile tile) {
        return false;
    }

    public override bool isShipDockingTile(Tile tile) {
        return false;
    }

    public override bool isCanoeMovableTile(Tile tile) {
        return false;
    }

    public override bool isAirshipLandable(Tile tile) {
        return false;
    }
}
