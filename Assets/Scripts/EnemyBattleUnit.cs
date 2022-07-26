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
    [SerializeField] float takingActionFlashSeconds;
    [SerializeField] float deathTransitionSeconds;
    [SerializeField] float battleEntranceOffsetX;
    [SerializeField] float entranceSeconds;

    private HitEffect hitEffect;
    private DamageAnimator damageAnimator;
    private BattleCalculator battleCalculator;
    private BattleMessageBar battleMessageBar;

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
        yield return animateTakingAction();

        if (battleCalculator.willEnemyRun(enemyUnit, battleContext)) {
            yield return battleMessageBar.displayMessage("Flee");
            yield return animateRunningAway();
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

    private IEnumerator animateRunningAway() {
        statsGameObject.SetActive(false);

        yield return unitImageRectTransform
            .DOAnchorPosX(unitImageStartX + 30f, 0.5f)
            .SetEase(Ease.OutQuad)
            .WaitForCompletion();

        yield return unitImageRectTransform
            .DOAnchorPosX(unitImageStartX + battleEntranceOffsetX, 0.5f)
            .SetEase(Ease.InQuad)
            .WaitForCompletion();
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

    private IEnumerator animateTakingAction() {
        for (int i = 0; i < 2; i++) {
            yield return unitImage
                .DOColor(Color.black, takingActionFlashSeconds / 2)
                .SetEase(Ease.Linear)
                .WaitForCompletion();
            yield return unitImage
                .DOColor(Color.white, takingActionFlashSeconds / 2)
                .SetEase(Ease.Linear)
                .WaitForCompletion();
        }

    }

    private IEnumerator die() {
        hpInfo.gameObject.SetActive(false);

        yield return unitImage
            .DOFade(0f, deathTransitionSeconds)
            .SetEase(Ease.Linear)
            .WaitForCompletion();
        disableUnit(EnemyBattleUnitState.KILLED);
    }

    public void setSelected(bool isSelected) {
        selectionCursor.setShowing(isSelected);
    }

    public IEnumerator enterBattle() {
        statsGameObject.SetActive(false);

        Vector2 battleEntranceInitialPosition = unitImageRectTransform.anchoredPosition;
        battleEntranceInitialPosition = new Vector2(
            battleEntranceInitialPosition.x + battleEntranceOffsetX,
            battleEntranceInitialPosition.y);
        unitImageRectTransform.anchoredPosition = battleEntranceInitialPosition;

        yield return unitImageRectTransform
            .DOAnchorPosX(unitImageStartX, entranceSeconds)
            .SetEase(Ease.OutExpo)
            .WaitForCompletion();

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
