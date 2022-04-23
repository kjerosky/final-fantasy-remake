using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

    private TileMovementData tileMovementData;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private Animator shipAnimator;
    private Animator airshipAnimator;

    private Vector3 movementTarget;
    private bool controlsEnabled;
    private MovementState movementState;
    private Dictionary<MovementState, float> movementStateToSpeed;

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

        spriteRenderer = GetComponent<SpriteRenderer>();

        if (ship != null) {
            shipAnimator = ship.gameObject.GetComponent<Animator>();
            resetShipAnimation();
        }

        if (airship != null) {
            airshipAnimator = airship.gameObject.GetComponent<Animator>();
        }

        movementTarget = NO_MOVEMENT_TARGET;
        controlsEnabled = false;
        movementState = MovementState.WALKING;

        movementStateToSpeed = new Dictionary<MovementState, float>();
        movementStateToSpeed.Add(MovementState.WALKING, walkSpeed);
        movementStateToSpeed.Add(MovementState.IN_CANOE, walkSpeed);
        movementStateToSpeed.Add(MovementState.IN_SHIP, shipSpeed);
        movementStateToSpeed.Add(MovementState.IN_AIRSHIP, airshipSpeed);
    }

    void Update() {
        if (movementTarget == NO_MOVEMENT_TARGET) {
            checkMovementInput();
        } else {
            moveTowardsMovementTarget();
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

    private void checkMovementInput() {
        if (!controlsEnabled) {
            return;
        }

        bool activateAirshipButtonWasPressed = Input.GetKeyDown(KeyCode.Space);
        if (airship != null && activateAirshipButtonWasPressed) {
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

        int horizontalMovementInput = (Input.GetKey(KeyCode.D) ? 1 : 0) - (Input.GetKey(KeyCode.A) ? 1 : 0);
        int verticalMovementInput = (Input.GetKey(KeyCode.W) ? 1 : 0) - (Input.GetKey(KeyCode.S) ? 1 : 0);

        Vector3 movementTargetCandidate = NO_MOVEMENT_TARGET;
        float horizontalDirection = 0;
        float verticalDirection = 0;
        if (horizontalMovementInput != 0) {
            movementTargetCandidate = transform.position;
            movementTargetCandidate.x = Mathf.Round(movementTargetCandidate.x + horizontalMovementInput);

            horizontalDirection = horizontalMovementInput;
            verticalDirection = 0;
        } else if (verticalMovementInput != 0) {
            movementTargetCandidate = transform.position;
            movementTargetCandidate.y = Mathf.Round(movementTargetCandidate.y + verticalMovementInput);

            horizontalDirection = 0;
            verticalDirection = verticalMovementInput;
        }

        if (movementTargetCandidate != NO_MOVEMENT_TARGET) {
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
        }
    }

    private void moveTowardsMovementTarget() {
        float moveSpeed = movementStateToSpeed[movementState];

        Vector3 newPosition = Vector3.MoveTowards(transform.position, movementTarget, moveSpeed * Time.deltaTime);
        transform.position = newPosition;

        if (newPosition != movementTarget) {
            return;
        }

        movementTarget = NO_MOVEMENT_TARGET;

        animator.SetBool("isWalking", false);

        Tile tileAtPlayerPosition = getTileAtWorldPosition(newPosition);
        if (ship != null && movementState == MovementState.IN_SHIP) {
            shipAnimator.SetBool("isMoving", false);
        } else if (
            ship != null &&
            (movementState == MovementState.WALKING || movementState == MovementState.IN_CANOE) &&
            newPosition == ship.position
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
            TransitionData transitionData = positionToTransitionData.getTransitionDataForTile(newPosition.x, newPosition.y);
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
