using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public class Player : MonoBehaviour {

    private enum MovementState {
        WALKING,
        IN_CANOE,
        IN_SHIP,
        IN_AIRSHIP
    };
    private static Vector3 NO_MOVEMENT_TARGET = Vector3.forward;
    private static int WRAPPING_X_MIN = 0;
    private static int WRAPPING_X_MAX = 255 + 20 + 20;
    private static int WRAPPING_Y_MIN = 0;
    private static int WRAPPING_Y_MAX = 255 + 20 + 20;

    public float walkSpeed = 4.0f;
    public float shipSpeed = 8.0f;
    public float airshipSpeed = 16.0f;
    public bool hasCanoe = false;
    public Tilemap tilemap;
    public Transform ship;
    public Transform airship;
    public LevelLoader levelLoader;
    public PositionToTransitionData positionToTransitionData;
    public LayerMask interactableLayer;
    public LayerMask solidObjectsLayer;

    private TileMovementData tileMovementData;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private Animator shipAnimator;
    private Animator airshipAnimator;

    private bool controlsEnabled;
    private MovementState movementState;
    private Dictionary<MovementState, float> movementStateToSpeed;
    private bool isMoving;
    private Vector3 facingDirection;
    private bool isOnWorldMap;

    void Awake() {
        SceneTransitionData sceneTransitionData = GameObject.Find("SceneTransitionData").GetComponent<SceneTransitionData>();
        Vector3 nextPlayerPosition = transform.position;
        nextPlayerPosition.x = sceneTransitionData.getNextPlayerX();
        nextPlayerPosition.y = sceneTransitionData.getNextPlayerY();
        transform.position = nextPlayerPosition;

        tileMovementData = tilemap.gameObject.GetComponent<TileMovementData>();

        animator = GetComponent<Animator>();
        animator.SetFloat("Horizontal", 0);
        animator.SetFloat("Vertical", -1);
        animator.SetBool("isWalking", false);
        animator.SetBool("isInCanoe", false);

        facingDirection = new Vector3(0, -1);

        spriteRenderer = GetComponent<SpriteRenderer>();

        if (ship != null) {
            shipAnimator = ship.gameObject.GetComponent<Animator>();
            resetShipAnimation();
        }

        if (airship != null) {
            airshipAnimator = airship.gameObject.GetComponent<Animator>();
        }

        controlsEnabled = false;
        movementState = MovementState.WALKING;

        movementStateToSpeed = new Dictionary<MovementState, float>();
        movementStateToSpeed.Add(MovementState.WALKING, walkSpeed);
        movementStateToSpeed.Add(MovementState.IN_CANOE, walkSpeed);
        movementStateToSpeed.Add(MovementState.IN_SHIP, shipSpeed);
        movementStateToSpeed.Add(MovementState.IN_AIRSHIP, airshipSpeed);

        isMoving = false;

        isOnWorldMap = SceneManager.GetActiveScene().name == "WorldMap";
    }

    public void handleUpdate() {
        if (controlsEnabled && !isMoving) {
            checkPlayerInput();
        }
    }

    public void onTransitionIntoSceneComplete() {
        controlsEnabled = true;
    }

    public void onAirshipTakeoffComplete() {
        controlsEnabled = true;
    }

    public void onAirshipLandingComplete() {
        animator.SetBool("isOnAirship", false);
        animator.SetFloat("Horizontal", 0);
        animator.SetFloat("Vertical", -1);

        airship.parent = null;
        movementState = MovementState.WALKING;
        controlsEnabled = true;
    }

    public void onAirshipShakingComplete() {
        controlsEnabled = true;
    }

    private void checkPlayerInput() {
        bool interactButtonWasPressed = Input.GetKeyDown(KeyCode.Space);
        if (airship != null && interactButtonWasPressed) {
            if (movementState == MovementState.WALKING && transform.position == airship.position) {
                animator.SetBool("isOnAirship", true);

                airshipAnimator.SetBool("playerIsOnAirship", true);
                airshipAnimator.SetFloat("Horizontal", 1);
                airshipAnimator.SetFloat("Vertical", 0);

                airship.parent = transform;
                movementState = MovementState.IN_AIRSHIP;
                controlsEnabled = false;
                return;
            } else if (movementState == MovementState.IN_AIRSHIP) {
                Tile currentTile = getTileAtWorldPosition(airship.position);
                if (tileMovementData.isAirshipLandable(currentTile)) {
                    airshipAnimator.SetBool("playerIsOnAirship", false);
                } else {
                    airshipAnimator.SetTrigger("Shake");
                }

                airshipAnimator.SetFloat("Horizontal", 1);
                airshipAnimator.SetFloat("Vertical", 0);
                controlsEnabled = false;
                return;
            }
        }

        if (interactButtonWasPressed) {
            Vector2 interactionTarget = transform.position + facingDirection;
            Collider2D interactionCollider = Physics2D.OverlapCircle(interactionTarget, 0.3f, interactableLayer);
            if (interactionCollider != null) {
                interactionCollider.GetComponent<Interactable>()?.interact();
                return;
            }
        }

        int horizontalMovementInput = (Input.GetKey(KeyCode.D) ? 1 : 0) - (Input.GetKey(KeyCode.A) ? 1 : 0);
        int verticalMovementInput = (Input.GetKey(KeyCode.W) ? 1 : 0) - (Input.GetKey(KeyCode.S) ? 1 : 0);
        if (horizontalMovementInput == 0 && verticalMovementInput == 0) {
            return;
        } else if (horizontalMovementInput != 0) {
            verticalMovementInput = 0;
        }

        Vector3 movementDirection = new Vector3(horizontalMovementInput, verticalMovementInput);
        facingDirection = movementDirection;
        
        animator.SetFloat("Horizontal", facingDirection.x);
        animator.SetFloat("Vertical", facingDirection.y);

        Vector3 moveToPosition = new Vector3(
            Mathf.Round(transform.position.x + movementDirection.x),
            Mathf.Round(transform.position.y + movementDirection.y)
        );

        if (!isWalkable(moveToPosition)) {
            return;
        }

        animator.SetBool("isWalking", true);

        if (!isOnWorldMap) {
            StartCoroutine(moveTowardsPosition(moveToPosition));
            return;
        }

        //TODO ADDITITONAL WORLD MAP MOVEMENT CHECKS SHOULD BE DONE HERE!!!
        StartCoroutine(moveTowardsPosition(moveToPosition));

        //TODO REWRITE THIS SECTION AND REMOVE IT!!!
        /*
        facingDirection.x = horizontalDirection;
        facingDirection.y = verticalDirection;
        animator.SetFloat("Horizontal", horizontalDirection);
        animator.SetFloat("Vertical", verticalDirection);
        if (ship != null && movementState == MovementState.IN_SHIP) {
            shipAnimator.SetFloat("Horizontal", horizontalDirection);
            shipAnimator.SetFloat("Vertical", verticalDirection);
        } else if (airship != null && movementState == MovementState.IN_AIRSHIP) {
            airshipAnimator.SetFloat("Horizontal", horizontalDirection);
            airshipAnimator.SetFloat("Vertical", verticalDirection);
        }

        Tile movementTargetCandidateTile = getTileAtWorldPosition(movementTargetCandidate);
        Vector3 movementTarget = NO_MOVEMENT_TARGET;
        switch (movementState) {
            case MovementState.WALKING: {
                if (
                    (ship != null && movementTargetCandidate == ship.position) ||
                    (hasCanoe && tileMovementData.isCanoeMovableTile(movementTargetCandidateTile)) ||
                    tileMovementData.isWalkableTile(movementTargetCandidateTile)
                ) {
                    movementTarget = movementTargetCandidate;
                    animator.SetBool("isWalking", true);
                }
            } break;

            case MovementState.IN_CANOE: {
                if (movementTargetCandidate == ship.position) {
                    movementTarget = movementTargetCandidate;
                    animator.SetBool("isWalking", true);
                    movementState = MovementState.WALKING;
                } else if (!tileMovementData.isCanoeMovableTile(movementTargetCandidateTile) && tileMovementData.isWalkableTile(movementTargetCandidateTile)) {
                    movementTarget = movementTargetCandidate;
                    animator.SetBool("isWalking", true);
                    animator.SetBool("isInCanoe", false);
                    movementState = MovementState.WALKING;
                } else if (tileMovementData.isCanoeMovableTile(movementTargetCandidateTile)) {
                    movementTarget = movementTargetCandidate;
                    animator.SetBool("isWalking", true);
                }
            } break;

            case MovementState.IN_SHIP: {

                if (tileMovementData.isShipMovableTile(movementTargetCandidateTile)) {
                    movementTarget = movementTargetCandidate;
                    shipAnimator.SetBool("isMoving", true);
                } else if (tileMovementData.isShipDockingTile(movementTargetCandidateTile)) {
                    movementTarget = movementTargetCandidate;
                    movementState = MovementState.WALKING;
                    disembarkFromShip();
                } else if (hasCanoe && tileMovementData.isCanoeMovableTile(movementTargetCandidateTile)) {
                    movementTarget = movementTargetCandidate;
                    movementState = MovementState.IN_CANOE;
                    disembarkFromShip();
                }
            } break;

            case MovementState.IN_AIRSHIP: {
                movementTarget = movementTargetCandidate;
            } break;
        }

        if (movementTarget != NO_MOVEMENT_TARGET) {
            StartCoroutine(moveTowardsPosition(movementTarget));
        }
        */
    }

    private bool isWalkable(Vector3 targetPosition) {
        return !Physics2D.OverlapCircle(targetPosition, 0.3f, solidObjectsLayer);
    }

    private IEnumerator moveTowardsPosition(Vector3 newPosition) {
        float moveSpeed = movementStateToSpeed[movementState];

        isMoving = true;
        while (transform.position != newPosition) {
            transform.position = Vector3.MoveTowards(transform.position, newPosition, moveSpeed * Time.deltaTime);
            yield return null;
        }
        isMoving = false;

        animator.SetBool("isWalking", false);

        Tile tileAtPlayerPosition = getTileAtWorldPosition(transform.position);
        if (ship != null && movementState == MovementState.IN_SHIP) {
            shipAnimator.SetBool("isMoving", false);
        } else if (
            ship != null &&
            (movementState == MovementState.WALKING || movementState == MovementState.IN_CANOE) &&
            transform.position == ship.position
        ) {
            movementState = MovementState.IN_SHIP;
            spriteRenderer.enabled = false;
            ship.parent = transform;
            shipAnimator.SetBool("isOnShip", true);
        }

        bool isOnCanoeTile = tileMovementData.isCanoeMovableTile(tileAtPlayerPosition);
        animator.SetBool("isInCanoe", isOnCanoeTile);
        if (movementState != MovementState.IN_AIRSHIP && isOnCanoeTile) {
            movementState = MovementState.IN_CANOE;
        }

        if (movementState == MovementState.WALKING || movementState == MovementState.IN_CANOE) {
            TransitionData transitionData = positionToTransitionData.getTransitionDataForTile(transform.position.x, transform.position.y);
            if (transitionData != null) {
                controlsEnabled = false;
                levelLoader.startTransition(transitionData.nextScene, transitionData.nextPlayerX, transitionData.nextPlayerY);
            }
        }

        adjustPlayerPositionWithWrapping();
    }

    private void adjustPlayerPositionWithWrapping() {
        // Only the world map will need this wrapping.  Other maps will be smaller than the world map
        // and be constructed in such a way that the player will never reach the map boundaries.

        if (transform.position.x < WRAPPING_X_MIN) {
            Vector3 adjustedPlayerPosition = transform.position;
            adjustedPlayerPosition.x = WRAPPING_X_MAX - (WRAPPING_X_MIN - transform.position.x - 1);
            transform.position = adjustedPlayerPosition;
        } else if (transform.position.x > WRAPPING_X_MAX) {
            Vector3 adjustedPlayerPosition = transform.position;
            adjustedPlayerPosition.x = WRAPPING_X_MIN + (transform.position.x - WRAPPING_X_MAX - 1);
            transform.position = adjustedPlayerPosition;
        }

        if (transform.position.y < WRAPPING_Y_MIN) {
            Vector3 adjustedPlayerPosition = transform.position;
            adjustedPlayerPosition.y = WRAPPING_Y_MAX - (WRAPPING_Y_MIN - transform.position.y - 1);
            transform.position = adjustedPlayerPosition;
        } else if (transform.position.y >= WRAPPING_Y_MAX) {
            Vector3 adjustedPlayerPosition = transform.position;
            adjustedPlayerPosition.y = WRAPPING_Y_MIN + (transform.position.y - WRAPPING_Y_MAX - 1);
            transform.position = adjustedPlayerPosition;
        }
    }

    private void disembarkFromShip() {
        spriteRenderer.enabled = true;
        ship.parent = null;
        resetShipAnimation();

        animator.SetBool("isWalking", true);
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
