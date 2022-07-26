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

    private EnemyUnit enemyUnit;
    private int teamMemberIndex;

    private WaitForSeconds delayBeforeAction;
    private bool isDoneActing;
    private RectTransform unitImageRectTransform;
    private float unitImageStartX;

    public bool IsEnemy => true;
    public int TeamMemberIndex => teamMemberIndex;
    public Sprite UnitActionQueueSprite => null;
    public int CurrentHp => enemyUnit.CurrentHp;

    public void setup(EnemyUnit enemyUnit, int teamMemberIndex) {
        this.enemyUnit = enemyUnit;
        this.teamMemberIndex = teamMemberIndex;

        hitEffect = GetComponent<HitEffect>();
        damageAnimator = GetComponent<DamageAnimator>();

        unitImage.sprite = enemyUnit.BattleSprite;

        hpInfo.setHp(enemyUnit.CurrentHp, enemyUnit.MaxHp);

        delayBeforeAction = new WaitForSeconds(delayBeforeActionSeconds);

        unitImageRectTransform = unitImage.GetComponent<RectTransform>();
        unitImageStartX = unitImageRectTransform.anchoredPosition.x;
    }

    public bool canAct() {
        return true;
    }

    public void prepareToAct(BattleContext battleContext) {
        isDoneActing = false;

        StartCoroutine(takeAction(battleContext));
    }

    public bool act() {
        return isDoneActing;
    }

    private IEnumerator takeAction(BattleContext battleContext) {
        List<BattleUnit> targetablePlayerUnits = battleContext.EnemyTargetablePlayerBattleUnits;
        int targetPlayerUnitIndex = Random.Range(0, targetablePlayerUnits.Count);
        BattleUnit targetPlayerUnit = targetablePlayerUnits[targetPlayerUnitIndex];

        yield return delayBeforeAction;

        yield return animateTakingAction();

        yield return targetPlayerUnit.takePhysicalDamage(this);

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
}
