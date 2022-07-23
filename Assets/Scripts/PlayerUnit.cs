using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PlayerUnit : Unit {

    private const float WALK_DISTANCE_X = 150f;

    private const float DAMAGE_KNOCKBACK_DISTANCE_X = 50f;
    private const float DAMAGE_KNOCKBACK_TOTAL_SECONDS = 0.4f;

    private const float WALKING_SECONDS = 0.25f;
    private Sprite[] walkingSprites;
    private float walkingFrameSeconds;

    private const float ATTACKING_SECONDS = 0.15f;
    private Sprite[] attackingSprites;
    private float attackingFrameSeconds;

    private PlayerUnitBase unitBase;
    private string name;
    private int currentHp;
    private int maxHp;

    private float startX;
    private float raisedWeaponStartX;

    public string Name => name;
    public int CurrentHp => currentHp;
    public int MaxHp => maxHp;
    public Sprite BattleIdleSprite => unitBase.BattleSpriteStanding;
    public Sprite UnitActionQueueSprite => unitBase.UnitActionQueueSprite;
    public List<BattleMenuCommand> BattleMenuCommands => unitBase.BattleMenuCommands;

    public PlayerUnit(PlayerUnitBase unitBase, string name) {
        this.unitBase = unitBase;
        this.name = name;
        maxHp = unitBase.Hp;
        currentHp = maxHp;

        walkingSprites = new Sprite[] {
            unitBase.BattleSpriteWalking,
            unitBase.BattleSpriteStanding,
            unitBase.BattleSpriteWalking,
            unitBase.BattleSpriteStanding
        };
        walkingFrameSeconds = WALKING_SECONDS / walkingSprites.Length;

        attackingSprites = new Sprite[] {
            unitBase.BattleSpriteWeaponRaised,
            unitBase.BattleSpriteWalking
        };
        attackingFrameSeconds = ATTACKING_SECONDS / attackingSprites.Length;
    }

    public IEnumerator beforeDealingDamage(Image unitImage, BattleWeapon battleWeapon, EnemyHitEffect enemyHitEffect) {
        RectTransform unitRectTransform = unitImage.GetComponent<RectTransform>();
        startX = unitRectTransform.anchoredPosition.x;
        float attackX = startX - WALK_DISTANCE_X;

        RectTransform raisedWeaponRectTransform = battleWeapon.RaisedWeaponImageRectTransform;
        raisedWeaponStartX = raisedWeaponRectTransform.anchoredPosition.x;
        float raisedWeaponAttackX = raisedWeaponStartX - WALK_DISTANCE_X;

        yield return animateWalking(unitImage, unitRectTransform, attackX, raisedWeaponRectTransform, raisedWeaponAttackX);

        yield return animateAttacking(unitImage, battleWeapon, enemyHitEffect);
    }

    public int takeDamage(BattleUnit attackingUnit) {
        //TODO REPLACE THIS WITH PROPERLY DETERMINED DAMAGE
        int TEMP_damageTaken = 5;

        currentHp = Mathf.Max(0, currentHp - TEMP_damageTaken);

        return TEMP_damageTaken;
    }

    public IEnumerator afterDealingDamage(Image unitImage, BattleWeapon battleWeapon) {
        RectTransform unitRectTransform = unitImage.GetComponent<RectTransform>();
        RectTransform raisedWeaponRectTransform = battleWeapon.RaisedWeaponImageRectTransform;

        unitRectTransform.localScale = new Vector3(-1f, 1f, 1f);

        yield return animateWalking(unitImage, unitRectTransform, startX, raisedWeaponRectTransform, raisedWeaponStartX);

        unitImage.sprite = unitBase.BattleSpriteStanding;
        unitRectTransform.localScale = new Vector3(1f, 1f, 1f);
    }

    public IEnumerator reactToBeingHit(Image unitImage) {
        RectTransform unitRectTransform = unitImage.GetComponent<RectTransform>();
        startX = unitRectTransform.anchoredPosition.x;
        float farRightX = startX + DAMAGE_KNOCKBACK_DISTANCE_X;
        float closerRightX = startX + DAMAGE_KNOCKBACK_DISTANCE_X / 2;

        Sequence shakeUnit = DOTween.Sequence()
            .Append(unitRectTransform
                .DOAnchorPosX(farRightX, DAMAGE_KNOCKBACK_TOTAL_SECONDS / 4)
                .SetEase(Ease.Linear)
            ).Append(unitRectTransform
                .DOAnchorPosX(startX, DAMAGE_KNOCKBACK_TOTAL_SECONDS / 4)
                .SetEase(Ease.Linear)
            ).Append(unitRectTransform
                .DOAnchorPosX(closerRightX, DAMAGE_KNOCKBACK_TOTAL_SECONDS / 4)
                .SetEase(Ease.Linear)
            ).Append(unitRectTransform
                .DOAnchorPosX(startX, DAMAGE_KNOCKBACK_TOTAL_SECONDS / 4)
                .SetEase(Ease.Linear)
            );

        yield return shakeUnit.WaitForCompletion();
    }

    public IEnumerator die(float transitionSeconds, Image unitImage, HpInfo unitHpInfo) {
        //TODO
        yield return null;
    }

    private IEnumerator animateWalking(
        Image unitImage,
        RectTransform unitImageRectTransform,
        float targetPositionX,
        RectTransform raisedWeaponImageRectTransform,
        float raisedWeaponTargetPositionX
    ) {
        unitImageRectTransform
            .DOAnchorPosX(targetPositionX, WALKING_SECONDS)
            .SetEase(Ease.Linear);

        raisedWeaponImageRectTransform
            .DOAnchorPosX(raisedWeaponTargetPositionX, WALKING_SECONDS)
            .SetEase(Ease.Linear);

        yield return animateUnitImage(unitImage, walkingSprites, walkingFrameSeconds);
    }

    private IEnumerator animateAttacking(Image unitImage, BattleWeapon battleWeapon, EnemyHitEffect enemyHitEffect) {
        enemyHitEffect.animate(ATTACKING_SECONDS);

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

        unitImage.sprite = unitBase.BattleSpriteStanding;
        battleWeapon.putAway();
    }

    private IEnumerator animateUnitImage(Image unitImage, Sprite[] animationFrames, float frameSeconds) {
        float timer = 0f;
        for (int i = 0; i < animationFrames.Length; i++) {
            unitImage.sprite = animationFrames[i];

            while (timer < frameSeconds) {
                timer += Time.deltaTime;
                yield return null;
            }

            while (timer >= frameSeconds) {
                timer -= frameSeconds;
            }
        }
    }
}
