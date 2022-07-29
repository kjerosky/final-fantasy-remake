using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BattleCalculator {

    public static bool willEnemyRun(EnemyUnit enemy, BattleContext battleContext) {
        int enemyMorale = enemy.Morale;
        int playerLeaderLevel = battleContext.PlayerBattleUnits
            .Where(unit => unit.canAct())
            .First()
            .Level;

        return enemyMorale - 2 * playerLeaderLevel + random(0, 50) < 80;
    }

    public static bool willEnemyUseMagic() {
        //TODO
        return false;
    }

    public static bool willEnemyUseSkill() {
        //TODO
        return false;
    }

    public static PlayerBattleUnit selectPositionWeightedRandomPlayer(BattleContext battleContext) {
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

    public static DamageCalculationResult calculatePhysicalDamage(Unit attacker, Unit defender) {
        int possibleNumberOfHits = Mathf.Max(1, attacker.NumberOfHits * attacker.HitMultiplier);

        int totalDamage = 0;
        int numberOfLandedHits = 0;
        bool criticalHitHappened = false;
        for (int i = 0; i < possibleNumberOfHits; i++) {
            int baseChanceToHit = 168;
            //TODO IF ATTACKER IS BLIND, SUBTRACT 40
            //TODO IF DEFENDER IS BLIND, ADD 40
            //TODO IF TARGET IS WEAK TO ATTACK (ATTACKER'S WEAPON ATTRIBUTE MATCHES
            //     ELEMENT WEAKNESS OR ENEMY TYPE OF DEFENDER), ADD 40

            //TODO IF DEFENDER IS ASLEEP OR PARALYZED, CALCULATE CHANCE TO HIT BELOW IGNORING DEFENDER'S EVASION
            int chanceToHit = baseChanceToHit + attacker.Accuracy - defender.Evasion;

            int hitRoll = random(0, 200);

            bool wasAutomaticMiss = hitRoll == 200;
            bool wasAutomaticHit = hitRoll == 0;
            bool hitCheckSucceeded = hitRoll <= chanceToHit;
            bool didHit = !wasAutomaticMiss && (wasAutomaticHit || hitCheckSucceeded);
            if (!didHit) {
                continue;
            }

            bool wasAutomaticNonCritical = hitRoll == 200;
            bool wasAutomaticCritical = hitRoll == 0;
            bool criticalHitCheckSucceeded = hitRoll <= attacker.CriticalRate;
            bool wasCritical = !wasAutomaticNonCritical && (wasAutomaticCritical || criticalHitCheckSucceeded);

            int attackValue = attacker.Attack;
            //TODO IF DEFENDER IS WEAK TO AN ELEMENTAL OR TYPE ATTRIBUTE OF THE ATTACKER'S WEAPON, ADD 4
            //TODO IF DEFENDER IS ASLEEP OR PARALYZED, MULTIPLY BY 5/4

            int baseDamage = random(attackValue, 2 * attackValue);
            int damage = Mathf.Max(1, baseDamage - defender.Defense);
            if (wasCritical) {
                damage += baseDamage;
            }

            //TODO CHANCE TO INFLICT STATUS EFFECTS

            totalDamage += damage;
            numberOfLandedHits++;
            criticalHitHappened |= wasCritical;
        }

        return new DamageCalculationResult(totalDamage, numberOfLandedHits, criticalHitHappened);
    }

    public static int random(int minInclusive, int maxInclusive) {
        return Random.Range(minInclusive, maxInclusive + 1);
    }
}

public class DamageCalculationResult {
    public int Damage { get; private set; }
    public int NumberOfHits { get; private set; }
    public bool WasCritical { get; private set; }

    public DamageCalculationResult(int damage, int numberOfHits, bool wasCritical) {
        Damage = damage;
        NumberOfHits = numberOfHits;
        WasCritical = wasCritical;
    }
}
