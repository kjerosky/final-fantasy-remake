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
    public LayerMask interactableLayer;
    public LayerMask solidObjectsLayer;
    public LayerMask portalsLayer;
    [SerializeField] LayerMask doorsLayer;
    public GameObject claimedMovePositionPrefab;
    [SerializeField] Tile openDoorTile;
    [SerializeField] Tile closedDoorTile;
    [SerializeField] bool hasShip;
    [SerializeField] bool hasAirship;
    [SerializeField] bool hasMysticKey;
    [SerializeField] GameObject interactionIcon;

    private Transform ship;
    private Transform airship;
    private WorldMapTileMovementData worldMapTileMovementData;
    private SpriteRenderer spriteRenderer;
    private PlayerAnimator animator;
    private Tilemap backgroundTilemap;
    private Tilemap roomsCoverTilemap;

    private bool controlsEnabled;
    private MovementState movementState;
    private Dictionary<MovementState, float> movementStateToSpeed;
    private bool isMoving;
    private Vector3 facingDirection;
    private bool isOnWorldMap;
    private PlayerSpritesType playerSpritesType;

    public bool HasMysticKey => hasMysticKey;

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

        playerSpritesType = PlayerSpritesType.FIGHTER;

        interactionIcon.SetActive(false);
    }

    public void handleBattleSceneUnloaded() {
        backgroundTilemap = GameObject.Find("Background").GetComponent<Tilemap>();
        roomsCoverTilemap = GameObject.Find("RoomsCover")?.GetComponent<Tilemap>();

        if (isOnWorldMap) {
            ship = GameObject.Find("Ship").transform;
            airship = GameObject.Find("Airship").transform;
        }
    }

    public void handleSceneLoaded(Vector3 newPlayerPosition) {
        transform.position = newPlayerPosition;

        backgroundTilemap = GameObject.Find("Background").GetComponent<Tilemap>();
        roomsCoverTilemap = GameObject.Find("RoomsCover")?.GetComponent<Tilemap>();

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

            GameObject airshipObject = GameObject.Find("Airship");
            airship = airshipObject.GetComponent<Transform>();
            airship.gameObject.SetActive(hasAirship);
            if (airship != null && airship.gameObject.activeSelf) {
                AirshipAnimator airshipAnimator = FindObjectOfType<AirshipAnimator>();
                airshipAnimator.OnEndTakeoff += () => {
                    controlsEnabled = true;
                };
                airshipAnimator.OnEndLanding += () => {
                    animator.PlayerSpritesType = playerSpritesType;
                    animator.Horizontal = 0;
                    animator.Vertical = -1;
                    animator.IsMoving = false;

                    airship.parent = null;
                    SceneManager.MoveGameObjectToScene(airship.gameObject, SceneManager.GetActiveScene());

                    movementState = MovementState.WALKING;
                    controlsEnabled = true;
                };
                airshipAnimator.OnEndShake += () => {
                    controlsEnabled = true;
                };
            }
        }
    }

    public void handleUpdate() {
        if (controlsEnabled && !isMoving) {
            interactionIcon.SetActive(isFacingInteractable());

            checkPlayerInput();
        }
    }

    private bool isFacingInteractable() {
        bool isFacingInteractable = false;

        Vector2 facingTarget = transform.position + facingDirection;
        Collider2D interactionCollider = Physics2D.OverlapCircle(
            facingTarget, 0.3f, interactableLayer | doorsLayer
        );
        if (interactionCollider != null) {
            Interactable interactable = interactionCollider.GetComponent<Interactable>();
            isFacingInteractable = interactable != null && interactable.isCurrentlyInteractable();
        }

        return isFacingInteractable;
    }

    private void checkPlayerInput() {
        //TODO REMOVE THIS TEMPORARY CODE!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        if (Input.GetKeyDown(KeyCode.B)) {
            FindObjectOfType<BattleTransitionManager>().transitionIntoBattle();
        }

        bool interactButtonWasPressed = Input.GetKeyDown(KeyCode.Space);

        if (isOnWorldMap && interactButtonWasPressed) {
            if (
                movementState == MovementState.WALKING &&
                airship.gameObject.activeSelf &&
                transform.position == airship.position
            ) {
                movementState = MovementState.IN_AIRSHIP;
                animator.PlayerSpritesType = PlayerSpritesType.AIRSHIP;

                airship.parent = transform;

                controlsEnabled = false;
                return;
            } else if (movementState == MovementState.IN_AIRSHIP) {
                Tile currentTile = getBackgroundTileAtWorldPosition(airship.position);
                if (currentTile != null && worldMapTileMovementData.isAirshipLandable(currentTile)) {
                    animator.signalAirshipToLand();
                } else {
                    animator.Horizontal = 1f;
                    animator.Vertical = 0f;
                    animator.signalAirshipToShake();
                }

                controlsEnabled = false;
                return;
            }
        }

        if (interactButtonWasPressed) {
            Vector2 interactionTarget = transform.position + facingDirection;
            Collider2D interactionCollider = Physics2D.OverlapCircle(
                interactionTarget, 0.3f, interactableLayer | doorsLayer
            );
            if (interactionCollider != null) {
                interactionCollider.GetComponent<Interactable>()?.interact(transform);
                interactionIcon.SetActive(false);
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

        if (!hasMysticKey && isLockedDoor(moveToPosition)) {
            return;
        }

        if (!isOnWorldMap) {
            StartCoroutine(moveTowardsPosition(moveToPosition));
            return;
        }

        if (checkAndProcessWorldMapMovability(moveToPosition)) {
            StartCoroutine(moveTowardsPosition(moveToPosition));
        }
    }

    private bool isWalkable(Vector3 targetPosition) {
        return !Physics2D.OverlapCircle(targetPosition, 0.3f, solidObjectsLayer | interactableLayer);
    }

    private bool isLockedDoor(Vector3 targetPosition) {
        bool isLocked = false;

        Collider2D door = Physics2D.OverlapCircle(targetPosition, 0.3f, doorsLayer);
        if (door != null) {
            isLocked = door.GetComponent<Door>().IsLocked;
        }

        return isLocked;
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
                    animator.PlayerSpritesType = playerSpritesType;
                    canMove = true;
                } else if (ship.gameObject.activeSelf && targetPosition == ship.position) {
                    movementState = MovementState.WALKING;
                    animator.PlayerSpritesType = playerSpritesType;
                    canMove = true;
                }
            } break;

            case MovementState.IN_SHIP: {
                if (worldMapTileMovementData.isShipMovableTile(targetTile)) {
                    canMove = true;
                } else if (worldMapTileMovementData.isShipDockingTile(targetTile)) {
                    movementState = MovementState.WALKING;
                    disembarkFromShip();
                    canMove = true;
                } else if (hasCanoe && worldMapTileMovementData.isCanoeMovableTile(targetTile)) {
                    movementState = MovementState.WALKING;
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
        interactionIcon.SetActive(false);

        Collider2D doorColliderAtStartPosition = Physics2D.OverlapCircle(transform.position, 0.3f, doorsLayer);
        Collider2D doorColliderAtNewPosition = Physics2D.OverlapCircle(newPosition, 0.3f, doorsLayer);
        if (doorColliderAtNewPosition != null) {
            doorColliderAtNewPosition.GetComponent<Door>().open();
            roomsCoverTilemap?.gameObject.SetActive(false);
        }

        float moveSpeed = movementStateToSpeed[movementState];

        GameObject claimedMoveDestination = Instantiate(claimedMovePositionPrefab, newPosition, Quaternion.identity);
        isMoving = true;
        animator.IsMoving = true;

        while (transform.position != newPosition) {
            transform.position = Vector3.MoveTowards(transform.position, newPosition, moveSpeed * Time.deltaTime);
            yield return null;
        }

        Destroy(claimedMoveDestination);
        isMoving = false;
        animator.IsMoving = false;

        if (doorColliderAtStartPosition != null) {
            Door door = doorColliderAtStartPosition.GetComponent<Door>();
            if (door != null && door.willCloseWithInitiatorPosition(newPosition)) {
                door.close();
                roomsCoverTilemap?.gameObject.SetActive(true);
            }
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

        if (movementState != MovementState.IN_AIRSHIP && ship.gameObject.activeSelf && transform.position == ship.position) {
            movementState = MovementState.IN_SHIP;
            boardShip();
        }

        bool isOnCanoeTile = false;
        Tile backgroundTileAtPlayerPosition = getBackgroundTileAtWorldPosition(transform.position);
        if (backgroundTileAtPlayerPosition != null) {
            isOnCanoeTile = worldMapTileMovementData.isCanoeMovableTile(backgroundTileAtPlayerPosition);
        }

        if (
            isOnCanoeTile &&
            movementState != MovementState.IN_AIRSHIP &&
            animator.PlayerSpritesType != PlayerSpritesType.CANOE
        ) {
            movementState = MovementState.IN_CANOE;
            animator.PlayerSpritesType = PlayerSpritesType.CANOE;
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

    private void boardShip() {
        animator.PlayerSpritesType = PlayerSpritesType.SHIP;
        animator.Horizontal = -1f;
        animator.Vertical = 0f;
        animator.IsMoving = false;

        ship.parent = transform;
        ship.gameObject.SetActive(false);
    }

    private void disembarkFromShip() {
        animator.PlayerSpritesType = playerSpritesType;

        ship.parent = null;
        ship.gameObject.SetActive(true);
        SceneManager.MoveGameObjectToScene(ship.gameObject, SceneManager.GetActiveScene());
    }

    private Tile getBackgroundTileAtWorldPosition(Vector3 worldPosition) {
        Vector3Int walkTargetCandidateCell = backgroundTilemap.WorldToCell(worldPosition);
        return backgroundTilemap.GetTile<Tile>(walkTargetCandidateCell);
    }
}
