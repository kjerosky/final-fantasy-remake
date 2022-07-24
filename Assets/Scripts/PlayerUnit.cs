using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PlayerUnit : Unit {

    private const float BATTLE_ENTRANCE_OFFSET_X = 300f;

    private const float ATTACK_WALK_OFFSET_X = -150f;

    private const float DAMAGE_KNOCKBACK_DISTANCE_X = 50f;
    private const float DAMAGE_KNOCKBACK_TOTAL_SECONDS = 0.4f;

    private const float ATTACK_WALKING_SECONDS = 0.25f;
    private const float WALKING_FRAME_SECONDS = 0.0625f;
    private Sprite[] walkingSprites;

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
    public Sprite BattleDeadSprite => unitBase.BattleSpriteDead;
    public Sprite UnitActionQueueSprite => unitBase.UnitActionQueueSprite;
    public List<BattleMenuCommand> BattleMenuCommands => unitBase.BattleMenuCommands;

    public PlayerUnit(PlayerUnitBase unitBase, string name) {
        this.unitBase = unitBase;
        this.name = name;
        maxHp = unitBase.Hp;
        currentHp = maxHp;

        walkingSprites = new Sprite[] {
            unitBase.BattleSpriteWalking,
            unitBase.BattleSpriteStanding
        };

        attackingSprites = new Sprite[] {
            unitBase.BattleSpriteWeaponRaised,
            unitBase.BattleSpriteWalking
        };
        attackingFrameSeconds = ATTACKING_SECONDS / attackingSprites.Length;
    }

    public IEnumerator enterBattle(BattleUnit myBattleUnit, float entranceSeconds) {
        Image unitImage = myBattleUnit.UnitImage;
        RectTransform unitImageRectTransform = unitImage.GetComponent<RectTransform>();
        BattleWeapon battleWeapon = myBattleUnit.GetComponent<BattleWeapon>();
        RectTransform raisedWeaponRectTransform = battleWeapon.RaisedWeaponImageRectTransform;

        startX = unitImageRectTransform.anchoredPosition.x;
        raisedWeaponStartX = raisedWeaponRectTransform.anchoredPosition.x;

        Vector2 battleEntranceInitialPosition = unitImageRectTransform.anchoredPosition;
        battleEntranceInitialPosition = new Vector2(
            battleEntranceInitialPosition.x + BATTLE_ENTRANCE_OFFSET_X,
            battleEntranceInitialPosition.y);
        unitImageRectTransform.anchoredPosition = battleEntranceInitialPosition;

        yield return animateWalking(
            unitImage,
            unitImageRectTransform,
            startX,
            raisedWeaponRectTransform,
            raisedWeaponStartX,
            entranceSeconds);

        unitImage.sprite = unitBase.BattleSpriteStanding;
    }

    public IEnumerator beforeDealingDamage(BattleUnit myBattleUnit, BattleUnit targetBattleUnit) {
        Image unitImage = myBattleUnit.UnitImage;
        BattleWeapon battleWeapon = myBattleUnit.GetComponent<BattleWeapon>();
        HitEffect enemyHitEffect = targetBattleUnit.GetComponent<HitEffect>();

        RectTransform unitRectTransform = unitImage.GetComponent<RectTransform>();
        float attackX = startX + ATTACK_WALK_OFFSET_X;

        RectTransform raisedWeaponRectTransform = battleWeapon.RaisedWeaponImageRectTransform;
        float raisedWeaponAttackX = raisedWeaponStartX + ATTACK_WALK_OFFSET_X;

        yield return animateWalking(
            unitImage,
            unitRectTransform,
            attackX,
            raisedWeaponRectTransform,
            raisedWeaponAttackX,
            ATTACK_WALKING_SECONDS);

        yield return animateAttacking(unitImage, battleWeapon, enemyHitEffect);
    }

    public int takeDamage(BattleUnit myBattleUnit, BattleUnit attackingBattleUnit) {
        //TODO REPLACE THIS WITH PROPERLY DETERMINED DAMAGE
        int TEMP_damageTaken = 40;

        currentHp = Mathf.Max(0, currentHp - TEMP_damageTaken);

        return TEMP_damageTaken;
    }

    public IEnumerator afterDealingDamage(BattleUnit myBattleUnit, BattleUnit targetBattleUnit) {
        Image unitImage = myBattleUnit.UnitImage;
        BattleWeapon battleWeapon = myBattleUnit.GetComponent<BattleWeapon>();

        RectTransform unitRectTransform = unitImage.GetComponent<RectTransform>();
        RectTransform raisedWeaponRectTransform = battleWeapon.RaisedWeaponImageRectTransform;

        unitRectTransform.localScale = new Vector3(-1f, 1f, 1f);

        yield return animateWalking(
            unitImage,
            unitRectTransform,
            startX,
            raisedWeaponRectTransform,
            raisedWeaponStartX,
            ATTACK_WALKING_SECONDS);

        unitImage.sprite = unitBase.BattleSpriteStanding;
        unitRectTransform.localScale = new Vector3(1f, 1f, 1f);
    }

    public IEnumerator reactToBeingHit(BattleUnit myBattleUnit, BattleUnit attackingBattleUnit) {
        Image unitImage = myBattleUnit.UnitImage;

        RectTransform unitRectTransform = unitImage.GetComponent<RectTransform>();
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

    public IEnumerator die(BattleUnit myBattleUnit, float transitionSeconds) {
        myBattleUnit.UnitImage.gameObject.SetActive(false);
        myBattleUnit.DeadImage.gameObject.SetActive(true);
        yield return null;
    }

    private IEnumerator animateWalking(
        Image unitImage,
        RectTransform unitImageRectTransform,
        float targetPositionX,
        RectTransform raisedWeaponImageRectTransform,
        float raisedWeaponTargetPositionX,
        float totalWalkSeconds
    ) {
        unitImageRectTransform
            .DOAnchorPosX(targetPositionX, totalWalkSeconds)
            .SetEase(Ease.Linear);

        raisedWeaponImageRectTransform
            .DOAnchorPosX(raisedWeaponTargetPositionX, totalWalkSeconds)
            .SetEase(Ease.Linear);

        yield return loopAnimateUnitImage(unitImage, walkingSprites, totalWalkSeconds, WALKING_FRAME_SECONDS);
    }

    private IEnumerator animateAttacking(Image unitImage, BattleWeapon battleWeapon, HitEffect enemyHitEffect) {
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

    private IEnumerator loopAnimateUnitImage(Image unitImage, Sprite[] animationFrames, float totalTime, float frameSeconds) {
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
