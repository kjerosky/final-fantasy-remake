using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class EnemyBattleUnit : MonoBehaviour {

    [SerializeField] EnemyUnitBase enemyUnitBase;
    [SerializeField] HpInfo hpInfo;
    [SerializeField] Image enemyUnitImage;
    [SerializeField] SelectionCursor selectionCursor;
    [SerializeField] float takeDamageSeconds;

    private EnemyUnit enemyUnit;

    public int CurrentHp => enemyUnit.CurrentHp;

    public void setup() {
        enemyUnit = new EnemyUnit(enemyUnitBase);

        hpInfo.setHp(enemyUnit.CurrentHp, enemyUnit.MaxHp);

        enemyUnitImage.sprite = enemyUnitBase.BattleSprite;

        setSelected(false);
    }

    public void setSelected(bool isSelected) {
        selectionCursor.setShowing(isSelected);
    }

    public IEnumerator takeDamagePhysical(PlayerBattleUnit attackingPlayerUnit) {
        enemyUnit.takeDamage(attackingPlayerUnit);

        yield return hpInfo.setHpSmooth(enemyUnit.CurrentHp, enemyUnit.MaxHp, takeDamageSeconds);
    }

    public IEnumerator die(float fadeOutSeconds) {
        hpInfo.gameObject.SetActive(false);

        yield return enemyUnitImage
            .DOFade(0f, fadeOutSeconds)
            .SetEase(Ease.Linear)
            .WaitForCompletion();
    }
}
