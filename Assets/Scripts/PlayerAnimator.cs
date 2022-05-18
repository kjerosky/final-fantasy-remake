using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour {

    [SerializeField] PlayerSpritesType playerSpritesType;
    [SerializeField] float walkFrameTime;
    [SerializeField] float canoeFrameTime;
    [SerializeField] float shipFrameTime;
    [SerializeField] float airshipFrameTime;

    public float Horizontal { get; set; }
    public float Vertical { get; set; }
    public bool IsMoving { get; set; }
    public PlayerSpritesType PlayerSpritesType {
        get => playerSpritesType;
        set {
            playerSpritesType = value;
            handleSpritesTypeChange();
        }
    }

    private SpriteRenderer spriteRenderer;
    private PlayerSpritesCollection playerSpritesCollection;

    private SpriteAnimator moveUpAnimator;
    private SpriteAnimator moveDownAnimator;
    private SpriteAnimator moveLeftAnimator;
    private SpriteAnimator moveRightAnimator;

    private SpriteAnimator currentAnimator;

    private bool wasPreviouslyMoving;

    void Start() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerSpritesCollection = GetComponent<PlayerSpritesCollection>();

        moveUpAnimator = new SpriteAnimator(spriteRenderer, null, 1f);
        moveDownAnimator = new SpriteAnimator(spriteRenderer, null, 1f);
        moveLeftAnimator = new SpriteAnimator(spriteRenderer, null, 1f);
        moveRightAnimator = new SpriteAnimator(spriteRenderer, null, 1f);

        currentAnimator = moveDownAnimator;

        handleSpritesTypeChange();

        wasPreviouslyMoving = false;
    }

    private void handleSpritesTypeChange() {
        List<List<Sprite>> playerSpritesList = playerSpritesCollection.getSpritesForType(playerSpritesType);
        moveUpAnimator.frames = playerSpritesList[0];
        moveDownAnimator.frames = playerSpritesList[1];
        moveLeftAnimator.frames = playerSpritesList[2];
        moveRightAnimator.frames = playerSpritesList[3];

        float frameTime = walkFrameTime;
        if (playerSpritesType == PlayerSpritesType.CANOE) {
            frameTime = canoeFrameTime;
        } else if (playerSpritesType == PlayerSpritesType.SHIP) {
            frameTime = shipFrameTime;
        } else if (playerSpritesType == PlayerSpritesType.AIRSHIP) {
            frameTime = airshipFrameTime;
        }

        moveUpAnimator.frameTime = frameTime;
        moveDownAnimator.frameTime = frameTime;
        moveLeftAnimator.frameTime = frameTime;
        moveRightAnimator.frameTime = frameTime;
    }

    void Update() {
        SpriteAnimator previousAnimator = currentAnimator;
        if (Horizontal == -1) {
            currentAnimator = moveLeftAnimator;
        } else if (Horizontal == 1) {
            currentAnimator = moveRightAnimator;
        } else if (Vertical == 1) {
            currentAnimator = moveUpAnimator;
        } else if (Vertical == -1) {
            currentAnimator = moveDownAnimator;
        }

        if (currentAnimator != previousAnimator || IsMoving != wasPreviouslyMoving) {
            currentAnimator.restart();
        }

        if (IsMoving) {
            currentAnimator.handleUpdate();
        } else {
            spriteRenderer.sprite = currentAnimator.frames[0];
        }

        wasPreviouslyMoving = IsMoving;
    }
}
