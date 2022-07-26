using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class EnemyBattleUnit : MonoBehaviour, BattleUnit {

    [SerializeField] Image unitImage;
    [SerializeField] HpInfo hpInfo;
    [SerializeField] SelectionCursor selectionCursor;
    [SerializeField] GameObject statsGameObject;
    [SerializeField] float delayBeforeActionSeconds;

    private HitEffect hitEffect;
    private DamageAnimator damageAnimator;
    private BattleCalculator battleCalculator;
    private BattleMessageBar battleMessageBar;
    private EnemyBattleUnitAnimator animator;

    private EnemyUnit enemyUnit;
    private int teamMemberIndex;

    private WaitForSeconds delayBeforeAction;
    private bool isDoneActing;
    private RectTransform unitImageRectTransform;
    private float unitImageStartX;
    private EnemyBattleUnitState state;

    public bool IsEnemy => true;
    public int TeamMemberIndex => teamMemberIndex;
    public Sprite UnitActionQueueSprite => null;
    public int CurrentHp => enemyUnit.CurrentHp;
    public int Gil => enemyUnit.Gil;
    public int Experience => enemyUnit.Experience;

    public void setup(EnemyUnit enemyUnit, int teamMemberIndex) {
        this.enemyUnit = enemyUnit;
        this.teamMemberIndex = teamMemberIndex;

        hitEffect = GetComponent<HitEffect>();
        damageAnimator = GetComponent<DamageAnimator>();
        battleCalculator = GetComponent<BattleCalculator>();
        battleMessageBar = BattleComponents.Instance.BattleMessageBar;
        animator = GetComponent<EnemyBattleUnitAnimator>();

        unitImage.sprite = enemyUnit.BattleSprite;

        hpInfo.setHp(enemyUnit.CurrentHp, enemyUnit.MaxHp);

        delayBeforeAction = new WaitForSeconds(delayBeforeActionSeconds);

        unitImageRectTransform = unitImage.GetComponent<RectTransform>();
        unitImageStartX = unitImageRectTransform.anchoredPosition.x;

        state = EnemyBattleUnitState.ALIVE;
    }

    public bool yieldsRewards() {
        return state == EnemyBattleUnitState.KILLED;
    }

    public bool canAct() {
        return state == EnemyBattleUnitState.ALIVE;
    }

    public void prepareToAct(BattleContext battleContext) {
        isDoneActing = false;

        StartCoroutine(takeAction(battleContext));
    }

    public bool act() {
        return isDoneActing;
    }

    private IEnumerator takeAction(BattleContext battleContext) {
        yield return delayBeforeAction;
        yield return animator.animateTakingAction();

        if (battleCalculator.willEnemyRun(enemyUnit, battleContext)) {
            yield return battleMessageBar.displayMessage("Flee");
            statsGameObject.SetActive(false);
            yield return animator.animateRunningAway();
            disableUnit(EnemyBattleUnitState.RAN_AWAY);
        } else if (battleCalculator.willEnemyUseMagic()) {
            //TODO
        } else if (battleCalculator.willEnemyUseSkill()) {
            //TODO
        } else {
            PlayerBattleUnit targetPlayerUnit = battleCalculator.selectPositionWeightedRandomPlayer(battleContext);
            yield return targetPlayerUnit.takePhysicalDamage(this);
        }

        isDoneActing = true;
    }

    public IEnumerator takePhysicalDamage(BattleUnit attackingUnit) {
        int damage = determinePhysicalDamage(attackingUnit);
        enemyUnit.takeDamage(damage);

        yield return hitEffect.animate(damage);

        yield return damageAnimator.animateDamage(damage, enemyUnit.CurrentHp, enemyUnit.MaxHp);

        if (enemyUnit.CurrentHp <= 0) {
            yield return die();
        }
    }

    private int determinePhysicalDamage(BattleUnit attackingUnit) {
        //TODO REPLACE THIS WITH PROPERLY DETERMINED DAMAGE
        int TEMP_damageTaken = 5;
        return TEMP_damageTaken;
    }

    private IEnumerator die() {
        statsGameObject.SetActive(false);
        yield return animator.animateDeath();
        disableUnit(EnemyBattleUnitState.KILLED);
    }

    public void setSelected(bool isSelected) {
        selectionCursor.setShowing(isSelected);
    }

    public IEnumerator enterBattle() {
        statsGameObject.SetActive(false);
        yield return animator.animateBattleEntrance();
        statsGameObject.SetActive(true);
    }

    private void disableUnit(EnemyBattleUnitState reasonState) {
        state = reasonState;
        unitImage.enabled = false;
        statsGameObject.SetActive(false);
    }
}

public enum EnemyBattleUnitState {
    ALIVE,
    RAN_AWAY,
    KILLED
}
