using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BattleContext {

    private List<PlayerBattleUnit> playerBattleUnits;
    private List<EnemyBattleUnit> enemyBattleUnits;
    private List<PlayerBattleUnit> enemyTargetablePlayerBattleUnits;

    public List<PlayerBattleUnit> PlayerBattleUnits => playerBattleUnits;
    public List<EnemyBattleUnit> EnemyBattleUnits => enemyBattleUnits;
    public List<PlayerBattleUnit> EnemyTargetablePlayerBattleUnits => enemyTargetablePlayerBattleUnits;

    public void initialize(List<PlayerBattleUnit> playerBattleUnits, List<EnemyBattleUnit> enemyBattleUnits) {
        this.playerBattleUnits = playerBattleUnits;
        this.enemyBattleUnits = enemyBattleUnits;

        enemyTargetablePlayerBattleUnits = playerBattleUnits
            .Where(unit => unit.CurrentHp > 0)
            .ToList();
    }
}
