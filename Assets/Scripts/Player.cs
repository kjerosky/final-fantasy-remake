using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Player : MonoBehaviour {

    private static Vector3 NO_NEW_POSITION = Vector3.forward;

    public Tilemap tilemap;

    private SolidTiles solidTiles;

    void Awake() {
        solidTiles = tilemap.gameObject.GetComponent<SolidTiles>();
    }

    void Update() {
        int horizontalMovement = (Input.GetKeyDown(KeyCode.D) ? 1 : 0) - (Input.GetKeyDown(KeyCode.A) ? 1 : 0);
        int verticalMovement = (Input.GetKeyDown(KeyCode.W) ? 1 : 0) - (Input.GetKeyDown(KeyCode.S) ? 1 : 0);

        Vector3 newPosition = NO_NEW_POSITION;
        if (horizontalMovement != 0) {
            newPosition = transform.position;
            newPosition.x = Mathf.Round(newPosition.x + horizontalMovement);
        } else if (verticalMovement != 0) {
            newPosition = transform.position;
            newPosition.y = Mathf.Round(newPosition.y + verticalMovement);
        }

        if (newPosition != NO_NEW_POSITION) {
            Vector3Int newPositionCell = tilemap.WorldToCell(newPosition);
            Tile newPositionTile = tilemap.GetTile<Tile>(newPositionCell);
            if (!solidTiles.isSolidTile(newPositionTile)) {
                transform.position = newPosition;
            }
        }
    }
}
