using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BattleContext {

    private List<BattleUnit> playerBattleUnits;
    private List<BattleUnit> enemyTargetablePlayerBattleUnits;

    public List<BattleUnit> PlayerBattleUnits => playerBattleUnits;
    public List<BattleUnit> EnemyTargetablePlayerBattleUnits => enemyTargetablePlayerBattleUnits;

    public void initialize(List<BattleUnit> playerBattleUnits) {
        this.playerBattleUnits = playerBattleUnits;

        enemyTargetablePlayerBattleUnits = playerBattleUnits
            .Where(unit => unit.CurrentHp > 0)
            .ToList();
    }
}
