using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour, Interactable {

    [SerializeField] bool isLocked;
    [SerializeField] Dialog lockedDialog;
    [SerializeField] float lockedHueRotationSeconds;
    [SerializeField] [Range(0f, 1f)] float lockedColorSaturation;
    [SerializeField] Sprite doorClosedSprite;
    [SerializeField] Sprite doorOpenedSprite;
    [SerializeField] Transform closeDoorTrigger;

    private SpriteRenderer spriteRenderer;
    private DialogManager dialogManager;

    private float lockedColorHue;
    private bool isOpen;

    public bool IsLocked => isLocked;

    void Start() {
        spriteRenderer = GetComponent<SpriteRenderer>();

        lockedColorHue = 0;
        isOpen = false;
    }

    void Update() {
        if (!isLocked) {
            return;
        }

        float hueChangeRate = (1f - 0f) / lockedHueRotationSeconds;
        lockedColorHue += hueChangeRate * Time.deltaTime;
        while (lockedColorHue > 1f) {
            lockedColorHue -= 1f;
        }

        spriteRenderer.color = Color.HSVToRGB(lockedColorHue, lockedColorSaturation, 1f);
    }

    void OnDrawGizmos() {
        if (!isLocked) {
            return;
        }

        Vector3 upperLeft = transform.position + (-0.5f * Vector3.right) + (0.5f * Vector3.up);
        Vector3 upperRight = transform.position + (0.5f * Vector3.right) + (0.5f * Vector3.up);
        Vector3 lowerLeft = transform.position + (-0.5f * Vector3.right) + (-0.5f * Vector3.up);
        Vector3 lowerRight = transform.position + (0.5f * Vector3.right) + (-0.5f * Vector3.up);

        Gizmos.color = Color.red;

        Gizmos.DrawLine(upperLeft, upperRight);
        Gizmos.DrawLine(upperRight, lowerRight);
        Gizmos.DrawLine(lowerRight, lowerLeft);
        Gizmos.DrawLine(lowerLeft, upperLeft);

        Gizmos.DrawLine(upperLeft, lowerRight);
        Gizmos.DrawLine(lowerLeft, upperRight);
    }

    public void open() {
        spriteRenderer.sprite = doorOpenedSprite;
        isOpen = true;
    }

    public bool willCloseWithInitiatorPosition(Vector3 initiatorPosition) {
        return initiatorPosition == closeDoorTrigger.position;
    }

    public void close() {
        spriteRenderer.sprite = doorClosedSprite;
        isOpen = false;
    }

    public bool isCurrentlyInteractable() {
        bool playerHasMysticKey = FindObjectOfType<Player>().HasMysticKey;
        return !playerHasMysticKey && isLocked && !isOpen;
    }

    public void interact(Transform initiator) {
        if (!isCurrentlyInteractable()) {
            return;
        }

        if (dialogManager == null) {
            dialogManager = FindObjectOfType<DialogManager>();
        }

        StartCoroutine(dialogManager.showDialog(lockedDialog));
    }
}
