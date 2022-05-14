using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NpcController : MonoBehaviour, Interactable {

    private static readonly Vector3[] POSSIBLE_MOVE_DIRECTIONS = new Vector3[] { Vector3.up, Vector3.down, Vector3.left, Vector3.right };

    [SerializeField] float moveSpeed;
    [SerializeField] float timeBetweenMoves;
    [SerializeField] Dialog dialog;
    [SerializeField] LayerMask solidObjectsLayer;
    [SerializeField] LayerMask interactableLayer;
    [SerializeField] LayerMask playerLayer;
    [SerializeField] GameObject claimedMoveDestinationPrefab;

    private CharacterAnimator animator;
    private DialogManager dialogManager;

    private NpcState state;
    private float timeSinceLastMove;

    void Awake() {
        animator = GetComponent<CharacterAnimator>();
        animator.Horizontal = 0;
        animator.Vertical = -1;
        animator.IsMoving = false;

        state = NpcState.IDLE;
        timeSinceLastMove = 0f;
    }

    void Update() {
        if (state != NpcState.IDLE) {
            return;
        }

        Vector3 randomMoveDirection;
        timeSinceLastMove += Time.deltaTime;
        if (timeSinceLastMove > timeBetweenMoves) {
            timeSinceLastMove = 0f;

            List<Vector3> moveDirectionCandidates = POSSIBLE_MOVE_DIRECTIONS
                .Where(moveDirection => isWalkable(transform.position + moveDirection)).ToList();
            randomMoveDirection = moveDirectionCandidates[Random.Range(0, moveDirectionCandidates.Count)];
        } else {
            return;
        }

        StartCoroutine(move(randomMoveDirection));
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
        state = NpcState.MOVING;
        animator.IsMoving = true;

        while (transform.position != targetPosition) {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
            yield return null;
        }

        Destroy(claimedMoveDestination);
        state = NpcState.IDLE;
        animator.IsMoving = false;
    }

    private bool isWalkable(Vector3 targetPosition) {
        return !Physics2D.OverlapCircle(targetPosition, 0.3f, solidObjectsLayer | interactableLayer | playerLayer);
    }

    public void interact(Transform initiator) {
        if (dialogManager == null) {
            dialogManager = FindObjectOfType<DialogManager>();
        }

        if (state != NpcState.IDLE) {
            return;
        }

        state = NpcState.INTERACTING;
        lookTowards(initiator.position);
        StartCoroutine(dialogManager.showDialog(dialog, () => {
            timeSinceLastMove = 0f;
            state = NpcState.IDLE;
        }));
    }

    private void lookTowards(Vector3 lookPoint) {
        float xLookDirection = Mathf.Round(lookPoint.x - transform.position.x);
        float yLookDirection = Mathf.Round(lookPoint.y - transform.position.y);

        if (xLookDirection != 0 && yLookDirection != 0) {
            return;
        }

        animator.Horizontal = Mathf.Clamp(xLookDirection, -1f, 1f);
        animator.Vertical = Mathf.Clamp(yLookDirection, -1f, 1f);
    }
}

public enum NpcState {
    IDLE,
    MOVING,
    INTERACTING
}
