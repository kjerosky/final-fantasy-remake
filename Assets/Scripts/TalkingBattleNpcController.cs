using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkingBattleNpcController : MonoBehaviour, Interactable {

    [SerializeField] Dialog dialog;

    private CharacterAnimator animator;
    private DialogManager dialogManager;

    void Start() {
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
            //TODO SETUP BATTLE TRANSFER OBJECT HERE BEFORE TRANSITIONING INTO BATTLE

            FindObjectOfType<BattleTransitionManager>().transitionIntoBattle(transform);
        }));
    }

    private Vector3 determineLookDirection(Vector3 initiatorPosition) {
        return (initiatorPosition - transform.position).normalized;
    }
}
