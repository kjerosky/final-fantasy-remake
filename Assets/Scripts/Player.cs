using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Player : MonoBehaviour {

    private static Vector3 NO_WALK_TARGET = Vector3.forward;

    public float walkSpeed = 4.0f;
    public float shipSpeed = 8.0f;
    public bool hasCanoe = false;
    public Tilemap tilemap;
    public Transform ship;

    private SolidTiles solidTiles;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private Animator shipAnimator;

    private Vector3 walkTarget;
    private bool isOnShip;

    void Awake() {
        solidTiles = tilemap.gameObject.GetComponent<SolidTiles>();

        animator = GetComponent<Animator>();
        animator.SetFloat("Horizontal", 0);
        animator.SetFloat("Vertical", -1);
        animator.SetBool("isWalking", false);
        animator.SetBool("isInCanoe", false);

        spriteRenderer = GetComponent<SpriteRenderer>();

        shipAnimator = ship.gameObject.GetComponent<Animator>();
        resetShipAnimation();

        walkTarget = NO_WALK_TARGET;

        isOnShip = false;
    }

    void Update() {
        if (walkTarget == NO_WALK_TARGET) {
            checkWalkInput();
        } else {
            walkTowardsTarget();
        }
    }

    private void checkWalkInput() {
        int horizontalMovement = (Input.GetKey(KeyCode.D) ? 1 : 0) - (Input.GetKey(KeyCode.A) ? 1 : 0);
        int verticalMovement = (Input.GetKey(KeyCode.W) ? 1 : 0) - (Input.GetKey(KeyCode.S) ? 1 : 0);

        Vector3 walkTargetCandidate = NO_WALK_TARGET;
        if (horizontalMovement != 0) {
            walkTargetCandidate = transform.position;
            walkTargetCandidate.x = Mathf.Round(walkTargetCandidate.x + horizontalMovement);

            animator.SetFloat("Horizontal", horizontalMovement);
            animator.SetFloat("Vertical", 0);
            if (isOnShip) {
                shipAnimator.SetFloat("Horizontal", horizontalMovement);
                shipAnimator.SetFloat("Vertical", 0);
            }
        } else if (verticalMovement != 0) {
            walkTargetCandidate = transform.position;
            walkTargetCandidate.y = Mathf.Round(walkTargetCandidate.y + verticalMovement);

            animator.SetFloat("Horizontal", 0);
            animator.SetFloat("Vertical", verticalMovement);
            if (isOnShip) {
                shipAnimator.SetFloat("Horizontal", 0);
                shipAnimator.SetFloat("Vertical", verticalMovement);
            }
        }

        if (walkTargetCandidate != NO_WALK_TARGET) {
            Tile walkTargetCandidateTile = getTileAtWorldPosition(walkTargetCandidate);
            if (!isOnShip && walkTargetCandidate == ship.position) {
                walkTarget = walkTargetCandidate;
                animator.SetBool("isWalking", true);
            } else if (isOnShip) {
                if (solidTiles.isShipMovableTile(walkTargetCandidateTile)) {
                    walkTarget = walkTargetCandidate;
                    shipAnimator.SetBool("isMoving", true);
                } else if (solidTiles.isShipDockingTile(walkTargetCandidateTile) || (hasCanoe && solidTiles.isCanoeWalkableTile(walkTargetCandidateTile))) {
                    isOnShip = false;
                    spriteRenderer.enabled = true;
                    ship.parent = null;
                    resetShipAnimation();

                    walkTarget = walkTargetCandidate;
                    animator.SetBool("isWalking", true);
                }
            } else if (animator.GetBool("isInCanoe") && !solidTiles.isCanoeWalkableTile(walkTargetCandidateTile) && solidTiles.isWalkableTile(walkTargetCandidateTile)) {
                walkTarget = walkTargetCandidate;
                animator.SetBool("isWalking", true);
                animator.SetBool("isInCanoe", false);
            } else if (animator.GetBool("isInCanoe") && solidTiles.isCanoeWalkableTile(walkTargetCandidateTile)) {
                walkTarget = walkTargetCandidate;
                animator.SetBool("isWalking", true);
            } else if (hasCanoe && !animator.GetBool("isInCanoe") && solidTiles.isCanoeWalkableTile(walkTargetCandidateTile)) {
                walkTarget = walkTargetCandidate;
                animator.SetBool("isWalking", true);
            } else if (solidTiles.isWalkableTile(walkTargetCandidateTile)) {
                walkTarget = walkTargetCandidate;
                animator.SetBool("isWalking", true);
            }
        }
    }

    private void walkTowardsTarget() {
        float moveSpeed = walkSpeed;
        if (isOnShip) {
            moveSpeed = shipSpeed;
        }

        Vector3 newPosition = Vector3.MoveTowards(transform.position, walkTarget, moveSpeed * Time.deltaTime);
        transform.position = newPosition;

        if (newPosition == walkTarget) {
            walkTarget = NO_WALK_TARGET;

            animator.SetBool("isWalking", false);
            if (isOnShip) {
                shipAnimator.SetBool("isMoving", false);
            }

            if (!isOnShip && newPosition == ship.position) {
                isOnShip = true;
                spriteRenderer.enabled = false;
                ship.parent = transform;
                shipAnimator.SetBool("isOnShip", true);
            }

            Tile tileAtPlayerPosition = getTileAtWorldPosition(newPosition);
            animator.SetBool("isInCanoe", solidTiles.isCanoeWalkableTile(tileAtPlayerPosition));
        }
    }

    private void resetShipAnimation() {
        shipAnimator.SetFloat("Horizontal", -1);
        shipAnimator.SetFloat("Vertical", 0);
        shipAnimator.SetBool("isOnShip", false);
        shipAnimator.SetBool("isMoving", false);
    }

    private Tile getTileAtWorldPosition(Vector3 worldPosition) {
        Vector3Int walkTargetCandidateCell = tilemap.WorldToCell(worldPosition);
        return tilemap.GetTile<Tile>(walkTargetCandidateCell);
    }
}
