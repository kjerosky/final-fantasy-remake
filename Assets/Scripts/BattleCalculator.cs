using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BattleCalculator : MonoBehaviour {

    public bool willEnemyRun(EnemyUnit enemy, BattleContext battleContext) {
        int enemyMorale = enemy.Morale;
        int playerLeaderLevel = battleContext.PlayerBattleUnits
            .Where(unit => unit.canAct())
            .First()
            .Level;

        return enemyMorale - 2 * playerLeaderLevel + random(0, 50) < 80;
    }

    public bool willEnemyUseMagic() {
        //TODO
        return false;
    }

    public bool willEnemyUseSkill() {
        //TODO
        return false;
    }

    public PlayerBattleUnit selectPositionWeightedRandomPlayer(BattleContext battleContext) {
        List<PlayerBattleUnit> playerUnits = battleContext.PlayerBattleUnits;

        int selectedIndex = -1;
        while (selectedIndex == -1) {
            int candidateIndex = 0;

            int randomNumber = random(1, 8);
            if (randomNumber == 5 || randomNumber == 6) {
                candidateIndex = 1;
            } else if (randomNumber == 7) {
                candidateIndex = 2;
            } else if (randomNumber == 8) {
                candidateIndex = 3;
            }

            if (playerUnits[candidateIndex].canAct()) {
                selectedIndex = candidateIndex;
            }
        }

        return playerUnits[selectedIndex];
    }

    private int random(int minInclusive, int maxInclusive) {
        return Random.Range(minInclusive, maxInclusive + 1);
    }
}
