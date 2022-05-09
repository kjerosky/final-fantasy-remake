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
    public Tilemap backgroundTilemap;
    public Transform ship;
    public Transform airship;
    public LevelLoader levelLoader;
    public PositionToTransitionData positionToTransitionData;
    public LayerMask interactableLayer;
    public LayerMask solidObjectsLayer;

    private WorldMapTileMovementData worldMapTileMovementData;
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
        isOnWorldMap = SceneManager.GetActiveScene().name == "WorldMap";

        SceneTransitionData sceneTransitionData = GameObject.Find("SceneTransitionData").GetComponent<SceneTransitionData>();
        Vector3 nextPlayerPosition = transform.position;
        nextPlayerPosition.x = sceneTransitionData.getNextPlayerX();
        nextPlayerPosition.y = sceneTransitionData.getNextPlayerY();
        transform.position = nextPlayerPosition;

        worldMapTileMovementData = GetComponent<WorldMapTileMovementData>();

        animator = GetComponent<Animator>();
        animator.SetFloat("Horizontal", 0);
        animator.SetFloat("Vertical", -1);
        animator.SetBool("isWalking", false);
        animator.SetBool("isInCanoe", false);

        facingDirection = new Vector3(0, -1);

        spriteRenderer = GetComponent<SpriteRenderer>();

        if (isOnWorldMap) {
            shipAnimator = ship.gameObject.GetComponent<Animator>();
            resetShipAnimation();

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

        if (isOnWorldMap && interactButtonWasPressed) {
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
                Tile currentTile = getBackgroundTileAtWorldPosition(airship.position);
                if (currentTile != null && worldMapTileMovementData.isAirshipLandable(currentTile)) {
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

        if (!isWalkable(moveToPosition) && movementState != MovementState.IN_AIRSHIP) {
            return;
        }

        if (!isOnWorldMap) {
            StartCoroutine(moveTowardsPosition(moveToPosition));
            return;
        }

        if (movementState == MovementState.IN_SHIP) {
            shipAnimator.SetFloat("Horizontal", facingDirection.x);
            shipAnimator.SetFloat("Vertical", facingDirection.y);
        } else if (movementState == MovementState.IN_AIRSHIP) {
            airshipAnimator.SetFloat("Horizontal", facingDirection.x);
            airshipAnimator.SetFloat("Vertical", facingDirection.y);
        }

        if (checkAndProcessWorldMapMovability(moveToPosition)) {
            StartCoroutine(moveTowardsPosition(moveToPosition));
        }
    }

    private bool isWalkable(Vector3 targetPosition) {
        return !Physics2D.OverlapCircle(targetPosition, 0.3f, solidObjectsLayer);
    }

    private bool checkAndProcessWorldMapMovability(Vector3 targetPosition) {
        Tile targetTile = getBackgroundTileAtWorldPosition(targetPosition);

        bool canMove = false;
        switch (movementState) {
            case MovementState.WALKING: {
                if (
                    worldMapTileMovementData.isWalkableLand(targetTile) ||
                    (hasCanoe && worldMapTileMovementData.isCanoeMovableTile(targetTile)) ||
                    targetPosition == ship.position
                ) {
                    animator.SetBool("isWalking", true);
                    canMove = true;
                }
            } break;

            case MovementState.IN_CANOE: {
                if (worldMapTileMovementData.isCanoeMovableTile(targetTile)) {
                    animator.SetBool("isWalking", true);
                    canMove = true;
                } else if (worldMapTileMovementData.isWalkableLand(targetTile)) {
                    animator.SetBool("isWalking", true);
                    animator.SetBool("isInCanoe", false);
                    movementState = MovementState.WALKING;
                    canMove = true;
                } else if (targetPosition == ship.position) {
                    animator.SetBool("isWalking", true);
                    movementState = MovementState.WALKING;
                    canMove = true;
                }
            } break;

            case MovementState.IN_SHIP: {
                if (worldMapTileMovementData.isShipMovableTile(targetTile)) {
                    shipAnimator.SetBool("isMoving", true);
                    canMove = true;
                } else if (worldMapTileMovementData.isShipDockingTile(targetTile)) {
                    movementState = MovementState.WALKING;
                    disembarkFromShip();
                    canMove = true;
                } else if (hasCanoe && worldMapTileMovementData.isCanoeMovableTile(targetTile)) {
                    movementState = MovementState.IN_CANOE;
                    disembarkFromShip();
                    canMove = true;
                }
            } break;

            case MovementState.IN_AIRSHIP: {
                canMove = true;
            } break;
        }

        return canMove;
    }

    private IEnumerator moveTowardsPosition(Vector3 newPosition) {
        float moveSpeed = movementStateToSpeed[movementState];

        animator.SetBool("isWalking", true);

        isMoving = true;
        while (transform.position != newPosition) {
            transform.position = Vector3.MoveTowards(transform.position, newPosition, moveSpeed * Time.deltaTime);
            yield return null;
        }
        isMoving = false;

        animator.SetBool("isWalking", false);

        if (movementState == MovementState.WALKING || movementState == MovementState.IN_CANOE) {
            TransitionData transitionData = positionToTransitionData.getTransitionDataForTile(transform.position.x, transform.position.y);
            if (transitionData != null) {
                controlsEnabled = false;
                levelLoader.startTransition(transitionData.nextScene, transitionData.nextPlayerX, transitionData.nextPlayerY);
            }
        }

        if (!isOnWorldMap) {
            yield break;
        }

        if (movementState == MovementState.IN_SHIP) {
            shipAnimator.SetBool("isMoving", false);
        } else if (movementState != MovementState.IN_AIRSHIP && transform.position == ship.position) {
            movementState = MovementState.IN_SHIP;
            spriteRenderer.enabled = false;
            ship.parent = transform;
            shipAnimator.SetBool("isOnShip", true);
        }

        bool isOnCanoeTile = false;
        Tile backgroundTileAtPlayerPosition = getBackgroundTileAtWorldPosition(transform.position);
        if (backgroundTileAtPlayerPosition != null) {
            isOnCanoeTile = worldMapTileMovementData.isCanoeMovableTile(backgroundTileAtPlayerPosition);
        }

        animator.SetBool("isInCanoe", isOnCanoeTile);
        if (movementState != MovementState.IN_AIRSHIP && isOnCanoeTile) {
            movementState = MovementState.IN_CANOE;
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

    private Tile getBackgroundTileAtWorldPosition(Vector3 worldPosition) {
        Vector3Int walkTargetCandidateCell = backgroundTilemap.WorldToCell(worldPosition);
        return backgroundTilemap.GetTile<Tile>(walkTargetCandidateCell);
    }
}
