using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour {

    [SerializeField] Sprite doorClosedSprite;
    [SerializeField] Sprite doorOpenedSprite;
    [SerializeField] Transform closeDoorTrigger;

    private SpriteRenderer spriteRenderer;

    void Start() {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void open() {
        spriteRenderer.sprite = doorOpenedSprite;
    }

    public void closeIfInitiatorExited(Vector3 initiatorPosition) {
        if (initiatorPosition == closeDoorTrigger.position) {
            spriteRenderer.sprite = doorClosedSprite;
        }
    }
}
