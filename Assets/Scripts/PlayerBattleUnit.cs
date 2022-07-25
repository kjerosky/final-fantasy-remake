using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PlayerBattleUnit : MonoBehaviour, BattleUnit {

    [SerializeField] Image unitImage;
    [SerializeField] Text nameText;
    [SerializeField] HpInfo hpInfo;
    [SerializeField] float damageKnockbackDistanceX;
    [SerializeField] float damageKnockbackTotalSeconds;

    private PlayerUnit playerUnit;
    private int teamMemberIndex;

    private DamageAnimator damageAnimator;

    private RectTransform unitImageRectTransform;
    private float unitImageStartX;

    private PlayerBattleUnitState state;

    public bool IsEnemy => false;
    public int CurrentHp => playerUnit.CurrentHp;

    public void setup(PlayerUnit playerUnit, int teamMemberIndex) {
        this.playerUnit = playerUnit;
        this.teamMemberIndex = teamMemberIndex;

        unitImage.sprite = playerUnit.BattleSpriteStanding;

        nameText.text = playerUnit.Name;

        hpInfo.setHp(playerUnit.CurrentHp, playerUnit.MaxHp);

        unitImageRectTransform = unitImage.GetComponent<RectTransform>();
        unitImageStartX = unitImageRectTransform.anchoredPosition.x;

        damageAnimator = GetComponent<DamageAnimator>();
    }

    public bool canAct() {
        //TODO ACCOUNT FOR OTHER STATUSES HERE TOO
        return playerUnit.CurrentHp > 0;
    }

    public void prepareToAct(BattleContext battleContext) {
        state = PlayerBattleUnitState.TEMP_WAITING_FOR_INPUT;
        Debug.Log($"{name} is acting.  Waiting for input...");
    }

    public bool act(BattleContext battleContext) {
        if (state == PlayerBattleUnitState.TEMP_WAITING_FOR_INPUT) {
            handleTempWaitingForInput(battleContext);
        }

        return state == PlayerBattleUnitState.DONE;
    }

    private void handleTempWaitingForInput(BattleContext battleContext) {
        if (Input.GetKeyDown(KeyCode.Return)) {
            StartCoroutine(performTempAction(battleContext));
        }
    }

    private IEnumerator performTempAction(BattleContext battleContext) {
        state = PlayerBattleUnitState.BUSY;

        BattleUnit targetEnemyUnit = battleContext.EnemyBattleUnits[0];

        //TODO PERFORM WALKING AND ATTACK ANIMATIONS

        yield return targetEnemyUnit.takePhysicalDamage(this);

        state = PlayerBattleUnitState.DONE;
    }

    public IEnumerator takePhysicalDamage(BattleUnit attackingUnit) {
        int damage = determinePhysicalDamage(attackingUnit);
        playerUnit.takeDamage(damage);

        if (damage > 0) {
            yield return animateReactionToDamage();
        }

        yield return damageAnimator.animateDamage(damage, playerUnit.CurrentHp, playerUnit.MaxHp);

        if (playerUnit.CurrentHp <= 0) {
            //TODO PLAYER DEATH
        }
    }

    private int determinePhysicalDamage(BattleUnit attackingUnit) {
        //TODO REPLACE THIS WITH PROPERLY DETERMINED DAMAGE
        int TEMP_damageTaken = 5;
        return TEMP_damageTaken;
    }

    private IEnumerator animateReactionToDamage() {
        float farRightX = unitImageStartX + damageKnockbackDistanceX;
        float closerRightX = unitImageStartX + damageKnockbackDistanceX / 2;

        Sequence shakeUnit = DOTween.Sequence()
            .Append(unitImageRectTransform
                .DOAnchorPosX(farRightX, damageKnockbackTotalSeconds / 4)
                .SetEase(Ease.Linear)
            ).Append(unitImageRectTransform
                .DOAnchorPosX(unitImageStartX, damageKnockbackTotalSeconds / 4)
                .SetEase(Ease.Linear)
            ).Append(unitImageRectTransform
                .DOAnchorPosX(closerRightX, damageKnockbackTotalSeconds / 4)
                .SetEase(Ease.Linear)
            ).Append(unitImageRectTransform
                .DOAnchorPosX(unitImageStartX, damageKnockbackTotalSeconds / 4)
                .SetEase(Ease.Linear)
            );

        yield return shakeUnit.WaitForCompletion();
    }
}

public enum PlayerBattleUnitState {
    TEMP_WAITING_FOR_INPUT,
    BUSY,
    DONE
}
