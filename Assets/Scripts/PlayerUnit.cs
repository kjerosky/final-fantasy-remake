using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PlayerUnit : Unit {

    private const float DAMAGE_KNOCKBACK_DISTANCE_X = 50f;
    private const float DAMAGE_KNOCKBACK_TOTAL_SECONDS = 0.4f;

    private PlayerUnitBase unitBase;
    private string name;
    private int currentHp;
    private int maxHp;

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
    }

    public IEnumerator beforeDealingDamage(Image unitImage) {
        //TODO
        yield return null;
    }

    public int takeDamage(BattleUnit attackingUnit) {
        //TODO REPLACE THIS WITH PROPERLY DETERMINED DAMAGE
        int TEMP_damageTaken = 5;

        currentHp = Mathf.Max(0, currentHp - TEMP_damageTaken);

        return TEMP_damageTaken;
    }

    public IEnumerator reactToBeingHit(Image unitImage) {
        RectTransform unitRectTransform = unitImage.GetComponent<RectTransform>();
        float startX = unitRectTransform.anchoredPosition.x;
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
}
