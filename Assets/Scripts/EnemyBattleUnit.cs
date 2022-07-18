using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyBattleUnit : MonoBehaviour {

    [SerializeField] EnemyUnitBase enemyUnitBase;
    [SerializeField] HpInfo hpInfo;
    [SerializeField] Image enemyUnitImage;

    private EnemyUnit enemyUnit;

    public void setup() {
        enemyUnit = new EnemyUnit(enemyUnitBase);

        hpInfo.setHp(enemyUnit.CurrentHp, enemyUnit.MaxHp);

        enemyUnitImage.sprite = enemyUnitBase.BattleSprite;
    }
}
