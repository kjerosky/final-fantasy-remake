using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System;

public class SolidTiles : MonoBehaviour {

    public Tile[] solidTiles;

    public bool isSolidTile(Tile tile) {
        return Array.Exists<Tile>(solidTiles, currentSolidTile => tile == currentSolidTile);
    }
}
