using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BattleSystem : MonoBehaviour {

    [SerializeField] PlayerBattleUnit playerUnit1;
    [SerializeField] PlayerBattleUnit playerUnit2;
    [SerializeField] PlayerBattleUnit playerUnit3;
    [SerializeField] PlayerBattleUnit playerUnit4;
    [SerializeField] EnemyBattleUnit enemyUnitSmall1;
    [SerializeField] EnemyBattleUnit enemyUnitSmall2;

    //TODO REMOVE THESE TEMPORARY FIELDS
    [SerializeField] PlayerUnitBase playerUnitBase1;
    [SerializeField] PlayerUnitBase playerUnitBase2;
    [SerializeField] PlayerUnitBase playerUnitBase3;
    [SerializeField] PlayerUnitBase playerUnitBase4;
    [SerializeField] EnemyUnitBase enemyUnitBase1;
    [SerializeField] EnemyUnitBase enemyUnitBase2;

    private BattleSystemState state;
    private List<BattleUnit> actionQueueBattleUnits;
    private BattleUnit currentBattleUnit;

    private BattleContext battleContext;
    private List<BattleUnit> playerBattleUnits;
    private List<BattleUnit> enemyBattleUnits;

    void Start() {
        playerUnit1.setup(new PlayerUnit(playerUnitBase1, "Abraham"), 0);
        playerUnit2.setup(new PlayerUnit(playerUnitBase2, "Bobby"), 1);
        playerUnit3.setup(new PlayerUnit(playerUnitBase3, "Carly"), 2);
        playerUnit4.setup(new PlayerUnit(playerUnitBase4, "Diana"), 3);

        enemyUnitSmall1.setup(new EnemyUnit(enemyUnitBase1), 0);
        enemyUnitSmall2.setup(new EnemyUnit(enemyUnitBase2), 1);

        playerBattleUnits = new List<BattleUnit>() {
            playerUnit1,
            playerUnit2,
            playerUnit3,
            playerUnit4
        };

        enemyBattleUnits = new List<BattleUnit>() {
            enemyUnitSmall1,
            enemyUnitSmall2
        };

        battleContext = new BattleContext();

        initializeActionQueue();

        state = BattleSystemState.PROCESSING_UNITS;
    }

    void Update() {
        if (state == BattleSystemState.PROCESSING_UNITS) {
            bool currentBattleUnitIsDoneActing = currentBattleUnit.act(battleContext);
            if (currentBattleUnitIsDoneActing) {
                setupNextActionableUnit();
            }
        }
    }

    private void initializeActionQueue() {
        //TODO IMPLEMENT RANDOMIZED ORDER
        actionQueueBattleUnits = new List<BattleUnit>() {
            playerUnit1,
            enemyUnitSmall1,
            playerUnit2,
            enemyUnitSmall2,
            playerUnit3,
            playerUnit4
        };

        currentBattleUnit = actionQueueBattleUnits[0];
        if (!currentBattleUnit.canAct()) {
            setupNextActionableUnit();
        } else {
            initializeBattleContext();
            currentBattleUnit.prepareToAct(battleContext);
        }
    }

    private void setupNextActionableUnit() {
        actionQueueBattleUnits = actionQueueBattleUnits
            .Where(unit => !unit.IsEnemy || (unit.IsEnemy && unit.CurrentHp > 0))
            .ToList();

        do {
            actionQueueBattleUnits.RemoveAt(0);
            actionQueueBattleUnits.Add(currentBattleUnit);
            currentBattleUnit = actionQueueBattleUnits[0];
        } while (!currentBattleUnit.canAct());

        initializeBattleContext();
        currentBattleUnit.prepareToAct(battleContext);
    }

    private void initializeBattleContext() {
        List<BattleUnit> activeEnemyBattleUnits = enemyBattleUnits
            .Where(unit => unit.CurrentHp > 0)
            .ToList();

        battleContext.initialize(playerBattleUnits, activeEnemyBattleUnits);
    }
}

public enum BattleSystemState {
    START,
    PROCESSING_UNITS,
    VICTORY,
    DEFEAT
}
