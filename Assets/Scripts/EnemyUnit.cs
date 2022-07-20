using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyUnit {

    private EnemyUnitBase unitBase;
    private int currentHp;
    private int maxHp;

    public EnemyUnitBase Base => unitBase;
    public int MaxHp => maxHp;
    public int CurrentHp => currentHp;

    public EnemyUnit(EnemyUnitBase unitBase) {
        this.unitBase = unitBase;
        maxHp = unitBase.Hp;
        currentHp = maxHp;
    }

    public int takeDamage(PlayerBattleUnit attackingPlayerUnit) {
        //TODO REPLACE THIS WITH PROPERLY DETERMINED DAMAGE
        int TEMP_damageTaken = 5;

        currentHp = Mathf.Max(0, currentHp - TEMP_damageTaken);

        return TEMP_damageTaken;
    }
}
