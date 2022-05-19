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
    private static int WRAPPING_X_MIN = 0;
    private static int WRAPPING_X_MAX = 255 + 20 + 20;
    private static int WRAPPING_Y_MIN = 0;
    private static int WRAPPING_Y_MAX = 255 + 20 + 20;

    public float walkSpeed = 4.0f;
    public float shipSpeed = 8.0f;
    public float airshipSpeed = 16.0f;
    public bool hasCanoe = false;
    public Tilemap backgroundTilemap;
    public LayerMask interactableLayer;
    public LayerMask solidObjectsLayer;
    public LayerMask portalsLayer;
    public GameObject claimedMovePositionPrefab;
    [SerializeField] Tile openDoorTile;
    [SerializeField] Tile closedDoorTile;
    [SerializeField] bool hasShip;
    [SerializeField] bool hasAirship;

    private Transform ship;
    private Transform airship;
    private WorldMapTileMovementData worldMapTileMovementData;
    private SpriteRenderer spriteRenderer;
    private PlayerAnimator animator;
    // private Animator shipAnimator;
    // private Animator airshipAnimator;

    private bool controlsEnabled;
    private MovementState movementState;
    private Dictionary<MovementState, float> movementStateToSpeed;
    private bool isMoving;
    private Vector3 facingDirection;
    private bool isOnWorldMap;

    void Start() {
        Vector3 defaultScenePosition = FindObjectOfType<EssentialObjectsLoader>().DefaultPlayerPosition;
        handleSceneLoaded(defaultScenePosition);

        worldMapTileMovementData = GetComponent<WorldMapTileMovementData>();

        spriteRenderer = GetComponent<SpriteRenderer>();

        controlsEnabled = true;
        movementState = MovementState.WALKING;

        movementStateToSpeed = new Dictionary<MovementState, float>();
        movementStateToSpeed.Add(MovementState.WALKING, walkSpeed);
        movementStateToSpeed.Add(MovementState.IN_CANOE, walkSpeed);
        movementStateToSpeed.Add(MovementState.IN_SHIP, shipSpeed);
        movementStateToSpeed.Add(MovementState.IN_AIRSHIP, airshipSpeed);

        isMoving = false;
    }

    public void handleSceneLoaded(Vector3 newPlayerPosition) {
        transform.position = newPlayerPosition;

        backgroundTilemap = GameObject.Find("Background").GetComponent<Tilemap>();

        animator = GetComponent<PlayerAnimator>();
        animator.Horizontal = 0f;
        animator.Vertical = -1f;
        animator.IsMoving = false;

        facingDirection = new Vector3(0, -1);

        isOnWorldMap = SceneManager.GetActiveScene().name == "WorldMap";
        if (isOnWorldMap) {
            GameObject shipObject = GameObject.Find("Ship");
            ship = shipObject.GetComponent<Transform>();
            ship.gameObject.SetActive(hasShip);
            // resetShipAnimation();

            GameObject airshipObject = GameObject.Find("Airship");
            airship = airshipObject.GetComponent<Transform>();
            airship.gameObject.SetActive(hasAirship);
        }
    }

    public void handleUpdate() {
        if (controlsEnabled && !isMoving) {
            checkPlayerInput();
        }
    }

    public void onAirshipTakeoffComplete() {
        controlsEnabled = true;
    }

    public void onAirshipLandingComplete() {
        // animator.SetBool("isOnAirship", false);
        // animator.SetFloat("Horizontal", 0);
        // animator.SetFloat("Vertical", -1);

        // airship.parent = null;
        // movementState = MovementState.WALKING;
        // controlsEnabled = true;
    }

    public void onAirshipShakingComplete() {
        controlsEnabled = true;
    }

    private void checkPlayerInput() {
        //TODO REMOVE THIS DEBUG
        if (Input.GetKeyDown(KeyCode.J)) {
            if (animator.PlayerSpritesType == PlayerSpritesType.FIGHTER) {
                animator.PlayerSpritesType = PlayerSpritesType.THIEF;
            } else {
                animator.PlayerSpritesType = PlayerSpritesType.FIGHTER;
            }
        }

        bool interactButtonWasPressed = Input.GetKeyDown(KeyCode.Space);

        if (isOnWorldMap && interactButtonWasPressed) {
            if (movementState == MovementState.WALKING && transform.position == airship.position) {
                // animator.SetBool("isOnAirship", true);

                // airshipAnimator.SetBool("playerIsOnAirship", true);
                // airshipAnimator.SetFloat("Horizontal", 1);
                // airshipAnimator.SetFloat("Vertical", 0);

                // airship.parent = transform;
                // movementState = MovementState.IN_AIRSHIP;
                // controlsEnabled = false;
                return;
            } else if (movementState == MovementState.IN_AIRSHIP) {
                // Tile currentTile = getBackgroundTileAtWorldPosition(airship.position);
                // if (currentTile != null && worldMapTileMovementData.isAirshipLandable(currentTile)) {
                //     airshipAnimator.SetBool("playerIsOnAirship", false);
                // } else {
                //     airshipAnimator.SetTrigger("Shake");
                // }

                // airshipAnimator.SetFloat("Horizontal", 1);
                // airshipAnimator.SetFloat("Vertical", 0);
                // controlsEnabled = false;
                return;
            }
        }

        if (interactButtonWasPressed) {
            Vector2 interactionTarget = transform.position + facingDirection;
            Collider2D interactionCollider = Physics2D.OverlapCircle(interactionTarget, 0.3f, interactableLayer);
            if (interactionCollider != null) {
                interactionCollider.GetComponent<Interactable>()?.interact(transform);
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
        
        animator.Horizontal = facingDirection.x;
        animator.Vertical = facingDirection.y;

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
            // shipAnimator.SetFloat("Horizontal", facingDirection.x);
            // shipAnimator.SetFloat("Vertical", facingDirection.y);
        } else if (movementState == MovementState.IN_AIRSHIP) {
            // airshipAnimator.SetFloat("Horizontal", facingDirection.x);
            // airshipAnimator.SetFloat("Vertical", facingDirection.y);
        }

        if (checkAndProcessWorldMapMovability(moveToPosition)) {
            StartCoroutine(moveTowardsPosition(moveToPosition));
        }
    }

    private bool isWalkable(Vector3 targetPosition) {
        return !Physics2D.OverlapCircle(targetPosition, 0.3f, solidObjectsLayer | interactableLayer);
    }

    private bool checkAndProcessWorldMapMovability(Vector3 targetPosition) {
        Tile targetTile = getBackgroundTileAtWorldPosition(targetPosition);

        bool canMove = false;
        switch (movementState) {
            case MovementState.WALKING: {
                if (
                    worldMapTileMovementData.isWalkableLand(targetTile) ||
                    (hasCanoe && worldMapTileMovementData.isCanoeMovableTile(targetTile)) ||
                    (ship.gameObject.activeSelf && targetPosition == ship.position)
                ) {
                    canMove = true;
                }
            } break;

            case MovementState.IN_CANOE: {
                if (worldMapTileMovementData.isCanoeMovableTile(targetTile)) {
                    canMove = true;
                } else if (worldMapTileMovementData.isWalkableLand(targetTile)) {
                    movementState = MovementState.WALKING;
                    animator.PlayerSpritesType = PlayerSpritesType.FIGHTER; //TODO MAKE THIS THE CURRENT PLAYER TYPE!!!
                    canMove = true;
                } else if (ship.gameObject.activeSelf && targetPosition == ship.position) {
                    movementState = MovementState.WALKING;
                    canMove = true;
                }
            } break;

            case MovementState.IN_SHIP: {
                if (worldMapTileMovementData.isShipMovableTile(targetTile)) {
                    canMove = true;
                } else if (worldMapTileMovementData.isShipDockingTile(targetTile)) {
                    movementState = MovementState.WALKING;
                    // disembarkFromShip();
                    canMove = true;
                } else if (hasCanoe && worldMapTileMovementData.isCanoeMovableTile(targetTile)) {
                    movementState = MovementState.IN_CANOE;
                    // disembarkFromShip();
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
        Vector3 startPosition = transform.position;
        Tile startTile = getBackgroundTileAtWorldPosition(startPosition);
        Tile destinationTile = getBackgroundTileAtWorldPosition(newPosition);

        if (destinationTile == closedDoorTile) {
            setBackgroundTileAtWorldPosition(newPosition, openDoorTile);
        }

        float moveSpeed = movementStateToSpeed[movementState];

        GameObject claimedMoveDestination = Instantiate(claimedMovePositionPrefab, newPosition, Quaternion.identity);
        isMoving = true;
        animator.IsMoving = true;
        // animator.SetBool("isWalking", true);

        while (transform.position != newPosition) {
            transform.position = Vector3.MoveTowards(transform.position, newPosition, moveSpeed * Time.deltaTime);
            yield return null;
        }

        Destroy(claimedMoveDestination);
        isMoving = false;
        animator.IsMoving = false;

        if (startTile == openDoorTile) {
            setBackgroundTileAtWorldPosition(startPosition, closedDoorTile);
        }

        if (movementState == MovementState.WALKING || movementState == MovementState.IN_CANOE) {
            Collider2D portalCollider = Physics2D.OverlapCircle(transform.position, 0.3f, portalsLayer);
            if (portalCollider != null) {
                portalCollider.GetComponent<Portal>().OnPlayerTriggered(this);
            }
        }

        if (!isOnWorldMap) {
            yield break;
        }

        // if (movementState == MovementState.IN_SHIP) {
        //     shipAnimator.SetBool("isMoving", false);
        // } else if (movementState != MovementState.IN_AIRSHIP && transform.position == ship.position) {
        //     movementState = MovementState.IN_SHIP;
        //     spriteRenderer.enabled = false;
        //     ship.parent = transform;
        //     shipAnimator.SetBool("isOnShip", true);
        // }

        bool isOnCanoeTile = false;
        Tile backgroundTileAtPlayerPosition = getBackgroundTileAtWorldPosition(transform.position);
        if (backgroundTileAtPlayerPosition != null) {
            isOnCanoeTile = worldMapTileMovementData.isCanoeMovableTile(backgroundTileAtPlayerPosition);
        }

        if (isOnCanoeTile && animator.PlayerSpritesType != PlayerSpritesType.CANOE) {
            animator.PlayerSpritesType = PlayerSpritesType.CANOE;
        }
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

        // animator.SetBool("isWalking", true);
    }

    private void resetShipAnimation() {
        // shipAnimator.SetFloat("Horizontal", -1);
        // shipAnimator.SetFloat("Vertical", 0);
        // shipAnimator.SetBool("isOnShip", false);
        // shipAnimator.SetBool("isMoving", false);
    }

    private Tile getBackgroundTileAtWorldPosition(Vector3 worldPosition) {
        Vector3Int walkTargetCandidateCell = backgroundTilemap.WorldToCell(worldPosition);
        return backgroundTilemap.GetTile<Tile>(walkTargetCandidateCell);
    }

    private void setBackgroundTileAtWorldPosition(Vector3 worldPosition, Tile tile) {
        Vector3Int cellPosition = backgroundTilemap.WorldToCell(worldPosition);
        backgroundTilemap.SetTile(cellPosition, tile);
    }
}
