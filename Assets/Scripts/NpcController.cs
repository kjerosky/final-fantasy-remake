using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcController : MonoBehaviour, Interactable {

    [SerializeField] float moveSpeed;
    [SerializeField] Dialog dialog;
    [SerializeField] LayerMask solidObjectsLayer;
    [SerializeField] LayerMask interactableLayer;
    [SerializeField] LayerMask playerLayer;
    [SerializeField] GameObject claimedMoveDestinationPrefab;

    private CharacterAnimator animator;
    private DialogManager dialogManager;

    private bool isMoving;

    void Awake() {
        animator = GetComponent<CharacterAnimator>();
        animator.Horizontal = 0;
        animator.Vertical = -1;
        animator.IsMoving = false;

        isMoving = false;
    }

    void Update() {
        if (isMoving) {
            return;
        }

        Vector3 moveDirection = new Vector2(
            (Input.GetKey(KeyCode.L) ? 1 : 0) - (Input.GetKey(KeyCode.J) ? 1 : 0),
            (Input.GetKey(KeyCode.I) ? 1 : 0) - (Input.GetKey(KeyCode.K) ? 1 : 0)
        );
        if (moveDirection.x != 0) {
            moveDirection.y = 0;
        }

        if (moveDirection.sqrMagnitude > 0) {
            StartCoroutine(move(moveDirection));
        }
    }

    private IEnumerator move(Vector3 moveDirection) {
        animator.Horizontal = moveDirection.x;
        animator.Vertical = moveDirection.y;

        Vector3 targetPosition = new Vector3(
            Mathf.Round(transform.position.x + moveDirection.x),
            Mathf.Round(transform.position.y + moveDirection.y)
        );

        if(!isWalkable(targetPosition)) {
            yield break;
        }

        GameObject claimedMoveDestination = Instantiate(claimedMoveDestinationPrefab, targetPosition, Quaternion.identity);
        isMoving = true;
        animator.IsMoving = true;

        while (transform.position != targetPosition) {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
            yield return null;
        }

        Destroy(claimedMoveDestination);
        isMoving = false;
        animator.IsMoving = false;
    }

    private bool isWalkable(Vector3 targetPosition) {
        return !Physics2D.OverlapCircle(targetPosition, 0.3f, solidObjectsLayer | interactableLayer | playerLayer);
    }

    public void interact() {
        if (dialogManager == null) {
            dialogManager = FindObjectOfType<DialogManager>();
        }

        StartCoroutine(dialogManager.showDialog(dialog));
    }
}
