using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PlayerBattleUnitAnimator : MonoBehaviour {

    [SerializeField] Image unitImage;
    [SerializeField] RectTransform imagesBaseRectTransform;
    [SerializeField] float battleEntranceOffsetX;
    [SerializeField] float entranceSeconds;
    [SerializeField] float walkingFrameSeconds;
    [SerializeField] float damageKnockbackDistanceX;
    [SerializeField] float damageKnockbackTotalSeconds;
    [SerializeField] float criticalDamageKnockbackDistanceX;
    [SerializeField] float criticalDamageKnockbackTotalSeconds;
    [SerializeField] float attackingSeconds;
    [SerializeField] float attackPositionOffsetX;
    [SerializeField] float attackWalkingSeconds;
    [SerializeField] float victoryPoseFrameSeconds;
    [SerializeField] float victoryWalkingOffsetX;
    [SerializeField] float victoryWalkingSeconds;

    private float imagesBaseStartX;
    private float imagesBaseAttackX;
    private BattleWeapon battleWeapon;

    private PlayerUnit unit;
    private Sprite[] walkingSprites;
    private Sprite[] attackingSprites;
    private float attackingFrameSeconds;

    void Start() {
        imagesBaseStartX = imagesBaseRectTransform.anchoredPosition.x;
        imagesBaseAttackX = imagesBaseStartX + attackPositionOffsetX;

        battleWeapon = GetComponent<BattleWeapon>();
    }

    public void setup(PlayerUnit unit) {
        this.unit = unit;

        walkingSprites = new Sprite[] {
            unit.BattleSpriteWalking,
            unit.BattleSpriteStanding
        };

        attackingSprites = new Sprite[] {
            unit.BattleSpriteWeaponRaised,
            unit.BattleSpriteWalking
        };
        attackingFrameSeconds = attackingSeconds / attackingSprites.Length;
    }

    public IEnumerator animateBattleEntrance(bool unitCanAct) {
        if (unitCanAct) {
            Vector2 battleEntranceInitialPosition = imagesBaseRectTransform.anchoredPosition;
            battleEntranceInitialPosition = new Vector2(
                battleEntranceInitialPosition.x + battleEntranceOffsetX,
                battleEntranceInitialPosition.y);
            imagesBaseRectTransform.anchoredPosition = battleEntranceInitialPosition;
        } else {
            yield return new WaitForSeconds(entranceSeconds);
        }

        yield return animateWalking(imagesBaseStartX, entranceSeconds);
    }

    public IEnumerator animateWalkingToAttackPoint() {
        yield return animateWalking(imagesBaseAttackX, attackWalkingSeconds);
        unitImage.sprite = unit.BattleSpriteStanding;
    }

    public IEnumerator animateAttacking() {
        float timer = 0f;
        unitImage.sprite = attackingSprites[0];
        battleWeapon.raise();
        while (timer < attackingFrameSeconds) {
            timer += Time.deltaTime;
            yield return null;
        }

        timer = 0f;
        unitImage.sprite = attackingSprites[1];
        battleWeapon.strike();
        while (timer < attackingFrameSeconds) {
            timer += Time.deltaTime;
            yield return null;
        }

        battleWeapon.putAway();
        unitImage.sprite = unit.BattleSpriteStanding;
    }

    public IEnumerator animateWalkingBackToStartPoint() {
        imagesBaseRectTransform.localScale = new Vector3(-1, 1, 1);
        yield return animateWalking(imagesBaseStartX, attackWalkingSeconds);
        imagesBaseRectTransform.localScale = new Vector3(1, 1, 1);
        unitImage.sprite = unit.BattleSpriteStanding;
    }

    public IEnumerator animateReactionToDamage() {
        float farRightX = imagesBaseStartX + damageKnockbackDistanceX;
        float closerRightX = imagesBaseStartX + damageKnockbackDistanceX / 2;

        Sequence shakeUnit = DOTween.Sequence()
            .Append(imagesBaseRectTransform
                .DOAnchorPosX(farRightX, damageKnockbackTotalSeconds / 4)
                .SetEase(Ease.Linear)
            ).Append(imagesBaseRectTransform
                .DOAnchorPosX(imagesBaseStartX, damageKnockbackTotalSeconds / 4)
                .SetEase(Ease.Linear)
            ).Append(imagesBaseRectTransform
                .DOAnchorPosX(closerRightX, damageKnockbackTotalSeconds / 4)
                .SetEase(Ease.Linear)
            ).Append(imagesBaseRectTransform
                .DOAnchorPosX(imagesBaseStartX, damageKnockbackTotalSeconds / 4)
                .SetEase(Ease.Linear)
            );

        yield return shakeUnit.WaitForCompletion();
    }

    public IEnumerator animateReactionToCriticalDamage() {
        float[] knockbackDistances = new float[] {
            criticalDamageKnockbackDistanceX,
            criticalDamageKnockbackDistanceX / 2,
            criticalDamageKnockbackDistanceX / 4,
            criticalDamageKnockbackDistanceX / 8
        };
        float shakeCycleTotalSeconds = criticalDamageKnockbackTotalSeconds / knockbackDistances.Length;

        Sequence shakeUnitCritically = DOTween.Sequence();
        for (int i = 0; i < knockbackDistances.Length; i++) {
            shakeUnitCritically
                .Append(imagesBaseRectTransform
                    .DOAnchorPosX(imagesBaseStartX + knockbackDistances[i], shakeCycleTotalSeconds / 4)
                    .SetEase(Ease.Linear)
                ).Append(imagesBaseRectTransform
                    .DOAnchorPosX(imagesBaseStartX - knockbackDistances[i], shakeCycleTotalSeconds / 2)
                    .SetEase(Ease.Linear)
                ).Append(imagesBaseRectTransform
                    .DOAnchorPosX(imagesBaseStartX, shakeCycleTotalSeconds / 4)
                    .SetEase(Ease.Linear)
                );
        }

        yield return shakeUnitCritically.WaitForCompletion();
    }

    public IEnumerator animateVictory(bool unitCanAct) {
        if (!unitCanAct) {
            yield break;
        }

        float timer;
        for (int i = 0; i < 2; i++) {
            unitImage.sprite = unit.BattleSpriteCasting;
            timer = 0f;
            while (timer < victoryPoseFrameSeconds) {
                timer += Time.deltaTime;
                yield return null;
            }

            unitImage.sprite = unit.BattleSpriteStanding;
            timer = 0f;
            while (timer < victoryPoseFrameSeconds) {
                timer += Time.deltaTime;
                yield return null;
            }
        }

        yield return animateWalking(imagesBaseStartX + victoryWalkingOffsetX, victoryWalkingSeconds);
    }

    private IEnumerator animateWalking(float targetPositionX, float totalWalkSeconds) {
        imagesBaseRectTransform
            .DOAnchorPosX(targetPositionX, totalWalkSeconds)
            .SetEase(Ease.Linear);

        yield return loopAnimateImage(unitImage, walkingSprites, totalWalkSeconds, walkingFrameSeconds);
        unitImage.sprite = unit.BattleSpriteStanding;
    }

    private IEnumerator loopAnimateImage(Image unitImage, Sprite[] animationFrames, float totalTime, float frameSeconds) {
        float totalTimeTimer = 0f;
        float frameTimer = 0f;

        while (true) {
            for (int i = 0; i < animationFrames.Length; i++) {
                unitImage.sprite = animationFrames[i];

                while (frameTimer < frameSeconds) {
                    totalTimeTimer += Time.deltaTime;
                    if (totalTimeTimer >= totalTime) {
                        yield break;
                    }

                    frameTimer += Time.deltaTime;

                    yield return null;
                }

                while (frameTimer >= frameSeconds) {
                    frameTimer -= frameSeconds;
                }
            }
        }
    }
}
