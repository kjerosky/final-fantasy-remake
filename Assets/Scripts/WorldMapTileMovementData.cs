using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System;

public class WorldMapTileMovementData : TileMovementData {

    public HashSet<string> walkableTileNames;
    public HashSet<string> shipMovableTileNames;
    public HashSet<string> shipDockingTileNames;
    public HashSet<string> canoeMovableTileNames;
    public HashSet<string> airshipLandableTileNames;

    void Awake() {
        walkableTileNames = new HashSet<string>();
        walkableTileNames.Add("ff1-world-tiles_0");
        walkableTileNames.Add("ff1-world-tiles_2");
        walkableTileNames.Add("ff1-world-tiles_6");
        walkableTileNames.Add("ff1-world-tiles_7");
        walkableTileNames.Add("ff1-world-tiles_8");
        walkableTileNames.Add("ff1-world-tiles_11");
        walkableTileNames.Add("ff1-world-tiles_15");
        walkableTileNames.Add("ff1-world-tiles_20");
        walkableTileNames.Add("ff1-world-tiles_21");
        walkableTileNames.Add("ff1-world-tiles_22");
        walkableTileNames.Add("ff1-world-tiles_23");
        walkableTileNames.Add("ff1-world-tiles_24");
        walkableTileNames.Add("ff1-world-tiles_25");
        walkableTileNames.Add("ff1-world-tiles_26");
        walkableTileNames.Add("ff1-world-tiles_27");
        walkableTileNames.Add("ff1-world-tiles_28");
        walkableTileNames.Add("ff1-world-tiles_30");
        walkableTileNames.Add("ff1-world-tiles_34");
        walkableTileNames.Add("ff1-world-tiles_35");
        walkableTileNames.Add("ff1-world-tiles_36");
        walkableTileNames.Add("ff1-world-tiles_39");
        walkableTileNames.Add("ff1-world-tiles_42");
        walkableTileNames.Add("ff1-world-tiles_43");
        walkableTileNames.Add("ff1-world-tiles_44");
        walkableTileNames.Add("ff1-world-tiles_45");
        walkableTileNames.Add("ff1-world-tiles_46");
        walkableTileNames.Add("ff1-world-tiles_56");
        walkableTileNames.Add("ff1-world-tiles_58");
        walkableTileNames.Add("ff1-world-tiles_59");
        walkableTileNames.Add("ff1-world-tiles_60");
        walkableTileNames.Add("ff1-world-tiles_65");
        walkableTileNames.Add("ff1-world-tiles_66");
        walkableTileNames.Add("ff1-world-tiles_68");
        walkableTileNames.Add("ff1-world-tiles_69");
        walkableTileNames.Add("ff1-world-tiles_70");
        walkableTileNames.Add("ff1-world-tiles_72");
        walkableTileNames.Add("ff1-world-tiles_73");
        walkableTileNames.Add("ff1-world-tiles_74");
        walkableTileNames.Add("ff1-world-tiles_75");
        walkableTileNames.Add("ff1-world-tiles_76");
        walkableTileNames.Add("ff1-world-tiles_77");
        walkableTileNames.Add("ff1-world-tiles_84");
        walkableTileNames.Add("ff1-world-tiles_85");
        walkableTileNames.Add("ff1-world-tiles_86");
        walkableTileNames.Add("ff1-world-tiles_87");
        walkableTileNames.Add("ff1-world-tiles_88");
        walkableTileNames.Add("ff1-world-tiles_89");
        walkableTileNames.Add("ff1-world-tiles_92");
        walkableTileNames.Add("ff1-world-tiles_93");
        walkableTileNames.Add("ff1-world-tiles_94");
        walkableTileNames.Add("ff1-world-tiles_95");
        walkableTileNames.Add("ff1-world-tiles_100");
        walkableTileNames.Add("ff1-world-tiles_102");
        walkableTileNames.Add("ff1-world-tiles_103");

        shipMovableTileNames = new HashSet<string>();
        shipMovableTileNames.Add("ff1-world-tiles_57");
        shipMovableTileNames.Add("ff1-world-tiles_1");
        shipMovableTileNames.Add("ff1-world-tiles_14");
        shipMovableTileNames.Add("ff1-world-tiles_16");
        shipMovableTileNames.Add("ff1-world-tiles_29");

        shipDockingTileNames = new HashSet<string>();
        shipDockingTileNames.Add("ff1-world-tiles_43");
        shipDockingTileNames.Add("ff1-world-tiles_56");
        shipDockingTileNames.Add("ff1-world-tiles_58");

        canoeMovableTileNames = new HashSet<string>();
        canoeMovableTileNames.Add("ff1-world-tiles_47");
        canoeMovableTileNames.Add("ff1-world-tiles_48");
        canoeMovableTileNames.Add("ff1-world-tiles_49");
        canoeMovableTileNames.Add("ff1-world-tiles_61");
        canoeMovableTileNames.Add("ff1-world-tiles_62");
        canoeMovableTileNames.Add("ff1-world-tiles_63");

        airshipLandableTileNames = new HashSet<string>();
        airshipLandableTileNames.Add("ff1-world-tiles_11");
        airshipLandableTileNames.Add("ff1-world-tiles_15");
        airshipLandableTileNames.Add("ff1-world-tiles_25");
        airshipLandableTileNames.Add("ff1-world-tiles_75");
        airshipLandableTileNames.Add("ff1-world-tiles_77");
        airshipLandableTileNames.Add("ff1-world-tiles_87");
        airshipLandableTileNames.Add("ff1-world-tiles_88");
        airshipLandableTileNames.Add("ff1-world-tiles_89");
    }

    public override bool isWalkableTile(Tile tile) {
        return walkableTileNames.Contains(tile.name);
    }

    public override bool isShipMovableTile(Tile tile) {
        return shipMovableTileNames.Contains(tile.name);
    }

    public override bool isShipDockingTile(Tile tile) {
        return shipDockingTileNames.Contains(tile.name);
    }

    public override bool isCanoeMovableTile(Tile tile) {
        return canoeMovableTileNames.Contains(tile.name);
    }

    public override bool isAirshipLandable(Tile tile) {
        return airshipLandableTileNames.Contains(tile.name);
    }
}
