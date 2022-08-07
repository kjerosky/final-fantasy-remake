using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator {

    public float Horizontal { get; set; }
    public float Vertical { get; set; }
    public bool IsMoving { get; set; }
    public PlayerSpritesType PlayerSpritesType => playerSpritesType;
    public AirshipAnimator AirshipAnimator {
        set => airshipAnimator = value;
    }

    private SpriteRenderer spriteRenderer;

    private PlayerSpritesType playerSpritesType;

    private SpriteAnimator moveUpAnimator;
    private SpriteAnimator moveDownAnimator;
    private SpriteAnimator moveLeftAnimator;
    private SpriteAnimator moveRightAnimator;

    private SpriteAnimator currentAnimator;
    private AirshipAnimator airshipAnimator;

    private bool wasPreviouslyMoving;

    private List<List<Sprite>> canoeFrames;
    private List<List<Sprite>> shipFrames;
    private List<List<Sprite>> airshipFrames;

    private float walkFrameTime;
    private float canoeFrameTime;
    private float shipFrameTime;
    private float airshipFrameTime;

    public PlayerAnimator(SpriteRenderer spriteRenderer, PlayerAnimatorParameters playerAnimatorParameters) {
        this.spriteRenderer = spriteRenderer;

        canoeFrames = new List<List<Sprite>>() {
            playerAnimatorParameters.CanoeMovingUpFrames,
            playerAnimatorParameters.CanoeMovingDownFrames,
            playerAnimatorParameters.CanoeMovingLeftFrames,
            playerAnimatorParameters.CanoeMovingRightFrames
        };
        shipFrames = new List<List<Sprite>>() {
            playerAnimatorParameters.ShipMovingUpFrames,
            playerAnimatorParameters.ShipMovingDownFrames,
            playerAnimatorParameters.ShipMovingLeftFrames,
            playerAnimatorParameters.ShipMovingRightFrames
        };
        airshipFrames = new List<List<Sprite>>() {
            playerAnimatorParameters.AirshipMovingUpFrames,
            playerAnimatorParameters.AirshipMovingDownFrames,
            playerAnimatorParameters.AirshipMovingLeftFrames,
            playerAnimatorParameters.AirshipMovingRightFrames
        };

        walkFrameTime = playerAnimatorParameters.WalkFrameTime;
        canoeFrameTime = playerAnimatorParameters.CanoeFrameTime;
        shipFrameTime = playerAnimatorParameters.ShipFrameTime;
        airshipFrameTime = playerAnimatorParameters.AirshipFrameTime;

        moveUpAnimator = new SpriteAnimator(spriteRenderer, null, 1f);
        moveDownAnimator = new SpriteAnimator(spriteRenderer, null, 1f);
        moveLeftAnimator = new SpriteAnimator(spriteRenderer, null, 1f);
        moveRightAnimator = new SpriteAnimator(spriteRenderer, null, 1f);

        currentAnimator = moveDownAnimator;

        wasPreviouslyMoving = false;
    }

    public void signalAirshipToLand() {
        airshipAnimator?.animateLanding();
    }

    public void signalAirshipToShake() {
        airshipAnimator?.animateShake();
    }

    public void changeSprites(PlayerSpritesType playerSpritesType, PlayerUnitBase unitBase) {
        this.playerSpritesType = playerSpritesType;

        if (playerSpritesType == PlayerSpritesType.CANOE) {
            moveUpAnimator.frames = canoeFrames[0];
            moveDownAnimator.frames = canoeFrames[1];
            moveLeftAnimator.frames = canoeFrames[2];
            moveRightAnimator.frames = canoeFrames[3];

            moveUpAnimator.frameTime = canoeFrameTime;
            moveDownAnimator.frameTime = canoeFrameTime;
            moveLeftAnimator.frameTime = canoeFrameTime;
            moveRightAnimator.frameTime = canoeFrameTime;
        } else if (playerSpritesType == PlayerSpritesType.SHIP) {
            moveUpAnimator.frames = shipFrames[0];
            moveDownAnimator.frames = shipFrames[1];
            moveLeftAnimator.frames = shipFrames[2];
            moveRightAnimator.frames = shipFrames[3];

            moveUpAnimator.frameTime = shipFrameTime;
            moveDownAnimator.frameTime = shipFrameTime;
            moveLeftAnimator.frameTime = shipFrameTime;
            moveRightAnimator.frameTime = shipFrameTime;
        } else if (playerSpritesType == PlayerSpritesType.AIRSHIP) {
            moveUpAnimator.frames = airshipFrames[0];
            moveDownAnimator.frames = airshipFrames[1];
            moveLeftAnimator.frames = airshipFrames[2];
            moveRightAnimator.frames = airshipFrames[3];

            moveUpAnimator.frameTime = airshipFrameTime;
            moveDownAnimator.frameTime = airshipFrameTime;
            moveLeftAnimator.frameTime = airshipFrameTime;
            moveRightAnimator.frameTime = airshipFrameTime;
        } else {
            moveUpAnimator.frames = unitBase.WalkUpSprites;
            moveDownAnimator.frames = unitBase.WalkDownSprites;
            moveLeftAnimator.frames = unitBase.WalkLeftSprites;
            moveRightAnimator.frames = unitBase.WalkRightSprites;

            moveUpAnimator.frameTime = walkFrameTime;
            moveDownAnimator.frameTime = walkFrameTime;
            moveLeftAnimator.frameTime = walkFrameTime;
            moveRightAnimator.frameTime = walkFrameTime;
        }

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

public enum PlayerSpritesType {
    PLAYER_UNIT,
    CANOE,
    SHIP,
    AIRSHIP
}
