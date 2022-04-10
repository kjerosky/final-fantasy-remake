using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Player : MonoBehaviour {

    private static Vector3 NO_WALK_TARGET = Vector3.forward;

    public float walkSpeed = 4.0f;
    public Tilemap tilemap;

    private SolidTiles solidTiles;
    private Animator animator;

    private Vector3 walkTarget;

    void Awake() {
        solidTiles = tilemap.gameObject.GetComponent<SolidTiles>();

        animator = GetComponent<Animator>();
        animator.SetFloat("Horizontal", 0);
        animator.SetFloat("Vertical", -1);
        animator.SetBool("isWalking", false);


        walkTarget = NO_WALK_TARGET;
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
        } else if (verticalMovement != 0) {
            walkTargetCandidate = transform.position;
            walkTargetCandidate.y = Mathf.Round(walkTargetCandidate.y + verticalMovement);

            animator.SetFloat("Horizontal", 0);
            animator.SetFloat("Vertical", verticalMovement);
        }

        if (walkTargetCandidate != NO_WALK_TARGET) {
            Vector3Int walkTargetCandidateCell = tilemap.WorldToCell(walkTargetCandidate);
            Tile walkTargetCandidateTile = tilemap.GetTile<Tile>(walkTargetCandidateCell);
            if (!solidTiles.isSolidTile(walkTargetCandidateTile)) {
                walkTarget = walkTargetCandidate;

                animator.SetBool("isWalking", true);
            }
        }
    }

    private void walkTowardsTarget() {
        Vector3 newPosition = Vector3.MoveTowards(transform.position, walkTarget, walkSpeed * Time.deltaTime);
        transform.position = newPosition;

        if (newPosition == walkTarget) {
            walkTarget = NO_WALK_TARGET;

            animator.SetBool("isWalking", false);
        }
    }
}
