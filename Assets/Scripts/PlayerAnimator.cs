using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator {

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
    public AirshipAnimator AirshipAnimator {
        set {
            airshipAnimator = value;
        }
    }

    private SpriteRenderer spriteRenderer;

    private PlayerSpritesCollection playerSpritesCollection;

    private PlayerSpritesType playerSpritesType;

    private SpriteAnimator moveUpAnimator;
    private SpriteAnimator moveDownAnimator;
    private SpriteAnimator moveLeftAnimator;
    private SpriteAnimator moveRightAnimator;

    private SpriteAnimator currentAnimator;
    private AirshipAnimator airshipAnimator;

    private bool wasPreviouslyMoving;

    private PlayerSprites playerSprites;
    private float walkFrameTime;
    private float canoeFrameTime;
    private float shipFrameTime;
    private float airshipFrameTime;

    public PlayerAnimator(SpriteRenderer spriteRenderer, PlayerAnimatorParameters playerAnimatorParameters) {
        this.spriteRenderer = spriteRenderer;

        playerSprites = playerAnimatorParameters.PlayerSprites;
        walkFrameTime = playerAnimatorParameters.WalkFrameTime;
        canoeFrameTime = playerAnimatorParameters.CanoeFrameTime;
        shipFrameTime = playerAnimatorParameters.ShipFrameTime;
        airshipFrameTime = playerAnimatorParameters.AirshipFrameTime;

        playerSpritesCollection = new PlayerSpritesCollection(playerSprites);

        moveUpAnimator = new SpriteAnimator(spriteRenderer, null, 1f);
        moveDownAnimator = new SpriteAnimator(spriteRenderer, null, 1f);
        moveLeftAnimator = new SpriteAnimator(spriteRenderer, null, 1f);
        moveRightAnimator = new SpriteAnimator(spriteRenderer, null, 1f);

        currentAnimator = moveDownAnimator;

        playerSpritesType = PlayerSpritesType.FIGHTER;
        handleSpritesTypeChange();

        wasPreviouslyMoving = false;
    }

    public void signalAirshipToLand() {
        airshipAnimator?.animateLanding();
    }

    public void signalAirshipToShake() {
        airshipAnimator?.animateShake();
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

        currentAnimator.restart();

        if (playerSpritesType == PlayerSpritesType.AIRSHIP) {
            Horizontal = 1f;
            Vertical = 0f;

            airshipAnimator?.animateTakeoff();
        }
    }

    public void handleUpdate() {
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

        airshipAnimator?.updateFacing(Horizontal, Vertical);

        wasPreviouslyMoving = IsMoving;
    }
}
