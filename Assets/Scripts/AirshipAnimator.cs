using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AirshipAnimator : MonoBehaviour {

    [SerializeField] float operationHeight;
    [SerializeField] float operationChangeSeconds;
    [SerializeField] float operationChangeFrameTime;
    [SerializeField] float moveFrameTime;
    [SerializeField] List<Sprite> moveUpFrames;
    [SerializeField] List<Sprite> moveDownFrames;
    [SerializeField] List<Sprite> moveLeftFrames;
    [SerializeField] List<Sprite> moveRightFrames;

    public float Horizontal { get; set; }
    public float Vertical { get; set; }

    public event Action OnEndTakeoff;
    public event Action OnEndLanding;

    private SpriteRenderer spriteRenderer;

    private SpriteAnimator onGroundAnimator;
    private SpriteAnimator operationAnimator;
    private SpriteAnimator moveUpAnimator;
    private SpriteAnimator moveDownAnimator;
    private SpriteAnimator moveLeftAnimator;
    private SpriteAnimator moveRightAnimator;

    private SpriteAnimator currentAnimator;

    private bool canMove;

    void Start() {
        spriteRenderer = GetComponent<SpriteRenderer>();

        canMove = false;

        onGroundAnimator = new SpriteAnimator(spriteRenderer, moveRightFrames.Take(1).ToList(), 0f);
        operationAnimator = new SpriteAnimator(spriteRenderer, moveRightFrames, operationChangeFrameTime);
        moveUpAnimator = new SpriteAnimator(spriteRenderer, moveUpFrames, moveFrameTime);
        moveDownAnimator = new SpriteAnimator(spriteRenderer, moveDownFrames, moveFrameTime);
        moveLeftAnimator = new SpriteAnimator(spriteRenderer, moveLeftFrames, moveFrameTime);
        moveRightAnimator = new SpriteAnimator(spriteRenderer, moveRightFrames, moveFrameTime);

        currentAnimator = onGroundAnimator;
        currentAnimator.restart();
    }

    void Update() {
        if (!canMove) {
            if (currentAnimator != onGroundAnimator) {
                currentAnimator.handleUpdate();
            }

            return;
        }

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

        if (currentAnimator != previousAnimator) {
            currentAnimator.restart();
        }

        currentAnimator.handleUpdate();
    }

    public void takeoff() {
        canMove = false;
        StartCoroutine(changeOperationHeight(0f, operationHeight));
    }

    public void land() {
        canMove = false;
        StartCoroutine(changeOperationHeight(operationHeight, 0f));
    }

    public void updateFacing(float horizontal, float vertical) {
        if (!canMove) {
            return;
        }

        Horizontal = horizontal;
        Vertical = vertical;
    }

    private IEnumerator changeOperationHeight(float startHeight, float endHeight) {
        Horizontal = 1f;
        Vertical = 0f;
        currentAnimator = operationAnimator;
        currentAnimator.restart();

        float heightChangeRate = Mathf.Abs(startHeight - endHeight) / operationChangeSeconds;

        transform.localPosition = new Vector3(0f, startHeight, 0f);
        Vector3 targetPosition = new Vector3(0f, endHeight, 0f);
        while (transform.localPosition != targetPosition) {
            transform.localPosition = Vector3.MoveTowards(
                transform.localPosition, targetPosition, heightChangeRate * Time.deltaTime
            );
            yield return null;
        }

        if (endHeight > 0f) {
            canMove = true;
            OnEndTakeoff?.Invoke();
        } else {
            currentAnimator = onGroundAnimator;
            currentAnimator.restart();

            OnEndLanding?.Invoke();
        }
    }
}
