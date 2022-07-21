using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUnit {

    private PlayerUnitBase unitBase;
    private int currentHp;
    private int maxHp;

    public PlayerUnitBase Base => unitBase;
    public int MaxHp => maxHp;
    public int CurrentHp => currentHp;

    public List<BattleMenuCommand> BattleMenuCommands => unitBase.BattleMenuCommands;

    public PlayerUnit(PlayerUnitBase unitBase) {
        this.unitBase = unitBase;
        maxHp = unitBase.Hp;
        currentHp = maxHp;
    }

    public int takeDamage(EnemyBattleUnit attackingEnemyUnit) {
        //TODO REPLACE THIS WITH PROPERLY DETERMINED DAMAGE
        int TEMP_damageTaken = 5;

        currentHp = Mathf.Max(0, currentHp - TEMP_damageTaken);

        return TEMP_damageTaken;
    }
}
