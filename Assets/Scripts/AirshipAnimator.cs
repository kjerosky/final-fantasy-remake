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
    [SerializeField] AnimationCurve shakeAnimationCurve;
    [SerializeField] float shakeTime;
    [SerializeField] float shakeAmplitude;
    [SerializeField] List<Sprite> moveUpFrames;
    [SerializeField] List<Sprite> moveDownFrames;
    [SerializeField] List<Sprite> moveLeftFrames;
    [SerializeField] List<Sprite> moveRightFrames;

    public float Horizontal { get; set; }
    public float Vertical { get; set; }

    public event Action OnEndTakeoff;
    public event Action OnEndLanding;
    public event Action OnEndShake;

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
        currentAnimator = determineMoveAnimatorFromFacing();

        if (currentAnimator != previousAnimator) {
            currentAnimator.restart();
        }

        currentAnimator.handleUpdate();
    }

    private SpriteAnimator determineMoveAnimatorFromFacing() {
        if (Horizontal == -1) {
            return moveLeftAnimator;
        } else if (Horizontal == 1) {
            return moveRightAnimator;
        } else if (Vertical == 1) {
            return moveUpAnimator;
        } else {
            return moveDownAnimator;
        }
    }

    public void animateTakeoff() {
        Horizontal = 1f;
        Vertical = 0f;
        canMove = false;

        currentAnimator = determineMoveAnimatorFromFacing();
        currentAnimator.restart();

        StartCoroutine(changeOperationHeight(0f, operationHeight));
    }

    public void animateLanding() {
        Horizontal = 1f;
        Vertical = 0f;
        canMove = false;

        currentAnimator = determineMoveAnimatorFromFacing();
        currentAnimator.restart();

        StartCoroutine(changeOperationHeight(operationHeight, 0f));
    }

    public void animateShake() {
        Horizontal = 1f;
        Vertical = 0f;
        canMove = false;

        currentAnimator = determineMoveAnimatorFromFacing();
        currentAnimator.restart();

        StartCoroutine(shake());
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

        float heightChangeRate = Mathf.Abs(endHeight - startHeight) / operationChangeSeconds;

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

    private IEnumerator shake() {
        Vector3 basePosition = transform.localPosition;

        float accumulatedTime = 0f;
        while (accumulatedTime < shakeTime) {
            float currentNormalizedTime = Mathf.InverseLerp(0f, shakeTime, accumulatedTime);
            float normalizedShakeAmount = shakeAnimationCurve.Evaluate(currentNormalizedTime);
            transform.localPosition = basePosition + shakeAmplitude * normalizedShakeAmount * Vector3.up;

            accumulatedTime += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = basePosition;
        OnEndShake?.Invoke();
        canMove = true;
    }
}
