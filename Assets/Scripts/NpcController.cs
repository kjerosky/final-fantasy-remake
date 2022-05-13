using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcController : MonoBehaviour, Interactable {

    [SerializeField] Dialog dialog;

    private CharacterAnimator animator;
    private DialogManager dialogManager;

    void Awake() {
        animator = GetComponent<CharacterAnimator>();
        animator.Horizontal = 0;
        animator.Vertical = -1;
        animator.IsMoving = false;
    }

    void Update() {
        if (Input.GetKey(KeyCode.J)) {
            animator.Horizontal = -1;
            animator.Vertical = 0;
            animator.IsMoving = true;
        } else if (Input.GetKey(KeyCode.L)) {
            animator.Horizontal = 1;
            animator.Vertical = 0;
            animator.IsMoving = true;
        } else if (Input.GetKey(KeyCode.I)) {
            animator.Horizontal = 0;
            animator.Vertical = 1;
            animator.IsMoving = true;
        } else if (Input.GetKey(KeyCode.K)) {
            animator.Horizontal = 0;
            animator.Vertical = -1;
            animator.IsMoving = true;
        } else {
            animator.IsMoving = false;
        }
    }

    public void interact() {
        if (dialogManager == null) {
            dialogManager = FindObjectOfType<DialogManager>();
        }

        StartCoroutine(dialogManager.showDialog(dialog));
    }
}
