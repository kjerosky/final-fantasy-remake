using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class EnemyBattleUnit : MonoBehaviour, BattleUnit {

    [SerializeField] Image unitImage;
    [SerializeField] HpInfo hpInfo;
    [SerializeField] float delayBeforeActionSeconds;
    [SerializeField] float takingActionFlashSeconds;

    private EnemyUnit enemyUnit;
    private int teamMemberIndex;

    private WaitForSeconds delayBeforeAction;
    private bool isDoneActing;

    public int CurrentHp => enemyUnit.CurrentHp;

    public void setup(EnemyUnit enemyUnit, int teamMemberIndex) {
        this.enemyUnit = enemyUnit;
        this.teamMemberIndex = teamMemberIndex;

        unitImage.sprite = enemyUnit.BattleSprite;

        hpInfo.setHp(enemyUnit.CurrentHp, enemyUnit.MaxHp);

        delayBeforeAction = new WaitForSeconds(delayBeforeActionSeconds);
    }

    public bool canAct() {
        return true;
    }

    public void prepareToAct(BattleContext battleContext) {
        isDoneActing = false;

        StartCoroutine(takeAction(battleContext));
    }

    public bool act(BattleContext battleContext) {
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
        yield return null;
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
}
