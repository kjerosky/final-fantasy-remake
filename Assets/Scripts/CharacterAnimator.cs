using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimator : MonoBehaviour {

    [SerializeField] float moveFrameTime;
    [SerializeField] List<Sprite> moveUpFrames;
    [SerializeField] List<Sprite> moveDownFrames;
    [SerializeField] List<Sprite> moveLeftFrames;
    [SerializeField] List<Sprite> moveRightFrames;

    public float Horizontal { get; set; }
    public float Vertical { get; set; }
    public bool IsMoving { get; set; }

    private SpriteRenderer spriteRenderer;

    private SpriteAnimator moveUpAnimator;
    private SpriteAnimator moveDownAnimator;
    private SpriteAnimator moveLeftAnimator;
    private SpriteAnimator moveRightAnimator;
    private SpriteAnimator currentAnimator;

    private bool wasPreviouslyMoving;

    void Start() {
        spriteRenderer = GetComponent<SpriteRenderer>();

        moveUpAnimator = new SpriteAnimator(spriteRenderer, moveUpFrames, moveFrameTime);
        moveDownAnimator = new SpriteAnimator(spriteRenderer, moveDownFrames, moveFrameTime);
        moveLeftAnimator = new SpriteAnimator(spriteRenderer, moveLeftFrames, moveFrameTime);
        moveRightAnimator = new SpriteAnimator(spriteRenderer, moveRightFrames, moveFrameTime);

        currentAnimator = moveDownAnimator;

        wasPreviouslyMoving = false;
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
            spriteRenderer.sprite = currentAnimator.Frames[0];
        }

        wasPreviouslyMoving = IsMoving;
    }
}
