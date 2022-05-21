using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AirshipAnimator : MonoBehaviour {

    [SerializeField] float operatingElevation;
    [SerializeField] float elevationChangeSeconds;
    [SerializeField] float elevationChangeFrameTime;
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
    private SpriteAnimator transitionAnimator;
    private SpriteAnimator moveUpAnimator;
    private SpriteAnimator moveDownAnimator;
    private SpriteAnimator moveLeftAnimator;
    private SpriteAnimator moveRightAnimator;

    private SpriteAnimator currentAnimator;

    private AirshipAnimatorState state;

    void Start() {
        spriteRenderer = GetComponent<SpriteRenderer>();

        state = AirshipAnimatorState.ON_GROUND;

        onGroundAnimator = new SpriteAnimator(spriteRenderer, moveRightFrames.Take(1).ToList(), 0f);
        transitionAnimator = new SpriteAnimator(spriteRenderer, moveRightFrames, elevationChangeFrameTime);
        moveUpAnimator = new SpriteAnimator(spriteRenderer, moveUpFrames, moveFrameTime);
        moveDownAnimator = new SpriteAnimator(spriteRenderer, moveDownFrames, moveFrameTime);
        moveLeftAnimator = new SpriteAnimator(spriteRenderer, moveLeftFrames, moveFrameTime);
        moveRightAnimator = new SpriteAnimator(spriteRenderer, moveRightFrames, moveFrameTime);

        currentAnimator = onGroundAnimator;
        currentAnimator.restart();
    }

    void Update() {
        if (state == AirshipAnimatorState.ON_GROUND) {
            return;
        } else if (state == AirshipAnimatorState.MOVING) {
            SpriteAnimator previousAnimator = currentAnimator;
            currentAnimator = determineMoveAnimatorFromFacing();

            if (currentAnimator != previousAnimator) {
                currentAnimator.restart();
            }
        }

        currentAnimator.handleUpdate();
    }

    public void animateTakeoff() {
        setupTransition();
        StartCoroutine(changeElevation(0f, operatingElevation));
    }

    public void animateLanding() {
        setupTransition();
        StartCoroutine(changeElevation(operatingElevation, 0f));
    }

    public void animateShake() {
        setupTransition();
        StartCoroutine(shake());
    }

    private void setupTransition() {
        Horizontal = 1f;
        Vertical = 0f;
        state = AirshipAnimatorState.TRANSITIONING;

        currentAnimator = determineMoveAnimatorFromFacing();
        currentAnimator.restart();
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

    public void updateFacing(float horizontal, float vertical) {
        if (state != AirshipAnimatorState.MOVING) {
            return;
        }

        Horizontal = horizontal;
        Vertical = vertical;
    }

    private IEnumerator changeElevation(float startElevation, float endElevation) {
        Horizontal = 1f;
        Vertical = 0f;
        currentAnimator = transitionAnimator;
        currentAnimator.restart();

        float elevationChangeRate = Mathf.Abs(endElevation - startElevation) / elevationChangeSeconds;

        transform.localPosition = new Vector3(0f, startElevation, 0f);
        Vector3 targetPosition = new Vector3(0f, endElevation, 0f);
        while (transform.localPosition != targetPosition) {
            transform.localPosition = Vector3.MoveTowards(
                transform.localPosition, targetPosition, elevationChangeRate * Time.deltaTime
            );
            yield return null;
        }

        if (endElevation > 0f) {
            state = AirshipAnimatorState.MOVING;
            OnEndTakeoff?.Invoke();
        } else {
            state = AirshipAnimatorState.ON_GROUND;
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
        state = AirshipAnimatorState.MOVING;
    }
}

public enum AirshipAnimatorState {
    ON_GROUND,
    TRANSITIONING,
    MOVING
}
