using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NpcController : MonoBehaviour, Interactable {

    private static readonly Vector3[] POSSIBLE_MOVE_DIRECTIONS = new Vector3[] { Vector3.up, Vector3.down, Vector3.left, Vector3.right };

    [SerializeField] bool movesAround;
    [SerializeField] float moveSpeed;
    [SerializeField] float minTimeBetweenMoves;
    [SerializeField] float maxTimeBetweenMoves;
    [SerializeField] float walkAreaOffsetMinX;
    [SerializeField] float walkAreaOffsetMaxX;
    [SerializeField] float walkAreaOffsetMinY;
    [SerializeField] float walkAreaOffsetMaxY;
    [SerializeField] Dialog dialog;
    [SerializeField] LayerMask solidObjectsLayer;
    [SerializeField] LayerMask interactableLayer;
    [SerializeField] LayerMask playerLayer;
    [SerializeField] GameObject claimedMoveDestinationPrefab;

    private CharacterAnimator animator;
    private DialogManager dialogManager;

    private NpcState state;
    private float timeSinceLastMove;
    private float timeToNextMove;

    private Vector3 nextPosition;
    private Vector3 previousPosition;
    private NpcState previousState;
    private float previousAnimatorHorizontal;
    private float previousAnimatorVertical;
    private bool previousAnimatorIsMoving;

    private float walkAreaMinX;
    private float walkAreaMaxX;
    private float walkAreaMinY;
    private float walkAreaMaxY;

    void Awake() {
        animator = GetComponent<CharacterAnimator>();
        animator.Horizontal = 0;
        animator.Vertical = -1;
        animator.IsMoving = !movesAround;

        state = NpcState.IDLE;
        timeSinceLastMove = 0f;
        timeToNextMove = determineTimeToNextMove();

        previousPosition = transform.position;
        nextPosition = transform.position;

        walkAreaMinX = transform.position.x + walkAreaOffsetMinX;
        walkAreaMaxX = transform.position.x + walkAreaOffsetMaxX;
        walkAreaMinY = transform.position.y + walkAreaOffsetMinY;
        walkAreaMaxY = transform.position.y + walkAreaOffsetMaxY;
    }

    void Update() {
        if (!movesAround || state != NpcState.IDLE) {
            return;
        }

        Vector3 randomMoveDirection;
        timeSinceLastMove += Time.deltaTime;
        if (timeSinceLastMove > timeToNextMove) {
            timeSinceLastMove = 0f;
            timeToNextMove = determineTimeToNextMove();

            List<Vector3> moveDirectionCandidates = POSSIBLE_MOVE_DIRECTIONS
                .Where(moveDirection => isWalkable(transform.position + moveDirection))
                .Where(moveDirection => determineIfMovingInDirectionIsInWalkArea(moveDirection))
                .ToList();
            randomMoveDirection = moveDirectionCandidates[Random.Range(0, moveDirectionCandidates.Count)];
        } else {
            return;
        }

        StartCoroutine(move(randomMoveDirection));
    }

    void OnDrawGizmosSelected() {
        if (!movesAround) {
            return;
        }

        Vector3 upperLeft = transform.position + ((walkAreaOffsetMinX - 0.5f) * Vector3.right) + ((walkAreaOffsetMaxY + 0.5f) * Vector3.up);
        Vector3 upperRight = transform.position + ((walkAreaOffsetMaxX + 0.5f) * Vector3.right) + ((walkAreaOffsetMaxY + 0.5f) * Vector3.up);
        Vector3 lowerLeft = transform.position + ((walkAreaOffsetMinX - 0.5f) * Vector3.right) + ((walkAreaOffsetMinY - 0.5f) * Vector3.up);
        Vector3 lowerRight = transform.position + ((walkAreaOffsetMaxX + 0.5f) * Vector3.right) + ((walkAreaOffsetMinY - 0.5f) * Vector3.up);

        Gizmos.color = Color.white;
        Gizmos.DrawLine(upperLeft, upperRight);
        Gizmos.DrawLine(upperRight, lowerRight);
        Gizmos.DrawLine(lowerRight, lowerLeft);
        Gizmos.DrawLine(lowerLeft, upperLeft);
    }

    private bool determineIfMovingInDirectionIsInWalkArea(Vector3 direction) {
        Vector3 destination = transform.position + direction;
        return (
            destination.x >= walkAreaMinX &&
            destination.x <= walkAreaMaxX &&
            destination.y >= walkAreaMinY &&
            destination.y <= walkAreaMaxY
        );
    }

    private float determineTimeToNextMove() {
        return Random.Range(minTimeBetweenMoves, maxTimeBetweenMoves);
    }

    private IEnumerator move(Vector3 moveDirection) {
        previousPosition = transform.position;

        animator.Horizontal = moveDirection.x;
        animator.Vertical = moveDirection.y;

        Vector3 targetPosition = new Vector3(
            Mathf.Round(transform.position.x + moveDirection.x),
            Mathf.Round(transform.position.y + moveDirection.y)
        );

        if(!isWalkable(targetPosition)) {
            yield break;
        }

        nextPosition = targetPosition;

        GameObject claimedMoveDestination = Instantiate(claimedMoveDestinationPrefab, targetPosition, Quaternion.identity);
        state = NpcState.MOVING;
        animator.IsMoving = true;

        while (transform.position != targetPosition) {
            if (state != NpcState.MOVING) {
                yield return null;
                continue;
            }

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

        previousState = state;
        previousAnimatorHorizontal = animator.Horizontal;
        previousAnimatorVertical = animator.Vertical;
        previousAnimatorIsMoving = animator.IsMoving;

        Vector3 lookDirection = determineLookDirection(initiator.position);

        state = NpcState.INTERACTING;
        animator.Horizontal = lookDirection.x;
        animator.Vertical = lookDirection.y;
        animator.IsMoving = false;
        StartCoroutine(dialogManager.showDialog(dialog, () => {
            timeSinceLastMove = 0f;

            state = previousState;
            animator.Horizontal = previousAnimatorHorizontal;
            animator.Vertical = previousAnimatorVertical;
            animator.IsMoving = previousAnimatorIsMoving;
        }));
    }

    private Vector3 determineLookDirection(Vector3 initiatorPosition) {
        Vector3 fromPreviousPositionToInitiator = (initiatorPosition - previousPosition).normalized;
        Vector3 fromNextPositionToInitiator = (initiatorPosition - nextPosition).normalized;
        if (Mathf.Abs(fromPreviousPositionToInitiator.x) == 1f || Mathf.Abs(fromPreviousPositionToInitiator.y) == 1f) {
            return fromPreviousPositionToInitiator;
        } else {
            return fromNextPositionToInitiator;
        }
    }
}

public enum NpcState {
    IDLE,
    MOVING,
    INTERACTING
}
