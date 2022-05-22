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

    public bool willCloseWithInitiatorPosition(Vector3 initiatorPosition) {
        return initiatorPosition == closeDoorTrigger.position;
    }

    public void close() {
        spriteRenderer.sprite = doorClosedSprite;
    }
}
