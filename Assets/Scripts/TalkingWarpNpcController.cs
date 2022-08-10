using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkingWarpNpcController : MonoBehaviour, Interactable {

    [SerializeField] Dialog dialog;
    [SerializeField] GameEvent eventToNotSpawn;
    [SerializeField] GameEvent eventToLogAfterSpeaking;
    [SerializeField] TravelDestination warpDestination;

    private CharacterAnimator animator;
    private DialogManager dialogManager;

    void Start() {
        if (GameEventsLog.Instance.isEventLogged(eventToNotSpawn)) {
            Destroy(gameObject);
            return;
        }

        animator = GetComponent<CharacterAnimator>();
        animator.Horizontal = 0;
        animator.Vertical = -1;
        animator.IsMoving = true;
    }

    public bool isCurrentlyInteractable() {
        return true;
    }

    public void interact(Transform initiator) {
        if (dialogManager == null) {
            dialogManager = FindObjectOfType<DialogManager>();
        }

        Vector3 lookDirection = determineLookDirection(initiator.position);
        animator.Horizontal = lookDirection.x;
        animator.Vertical = lookDirection.y;
        animator.IsMoving = false;

        StartCoroutine(dialogManager.showDialog(dialog, () => {
            GameEventsLog.Instance.logEvent(eventToLogAfterSpeaking);
            TravelHandler.Instance.travelTo(warpDestination);
        }));
    }

    private Vector3 determineLookDirection(Vector3 initiatorPosition) {
        return (initiatorPosition - transform.position).normalized;
    }
}
