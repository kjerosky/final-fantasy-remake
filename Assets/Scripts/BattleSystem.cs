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
    [SerializeField] BattleComponents battleComponents;

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
    private List<PlayerBattleUnit> playerBattleUnits;
    private List<EnemyBattleUnit> enemyBattleUnits;

    void Start() {
        playerUnit1.setup(new PlayerUnit(playerUnitBase1, "Abraham"), 0);
        playerUnit2.setup(new PlayerUnit(playerUnitBase2, "Bobby"), 1);
        playerUnit3.setup(new PlayerUnit(playerUnitBase3, "Carly"), 2);
        playerUnit4.setup(new PlayerUnit(playerUnitBase4, "Diana"), 3);

        enemyUnitSmall1.setup(new EnemyUnit(enemyUnitBase1), 0);
        enemyUnitSmall2.setup(new EnemyUnit(enemyUnitBase2), 1);

        playerBattleUnits = new List<PlayerBattleUnit>() {
            playerUnit1,
            playerUnit2,
            playerUnit3,
            playerUnit4
        };

        enemyBattleUnits = new List<EnemyBattleUnit>() {
            enemyUnitSmall1,
            enemyUnitSmall2
        };

        battleContext = new BattleContext();

        battleComponents.ActionQueue.gameObject.SetActive(false);
        battleComponents.BattleMenu.setShowingCommandsMenu(false);

        state = BattleSystemState.START;
    }

    void Update() {
        if (state == BattleSystemState.START) {
            StartCoroutine(performStart());
        } else if (state == BattleSystemState.PROCESSING_UNITS) {
            bool currentBattleUnitIsDoneActing = currentBattleUnit.act();
            if (currentBattleUnitIsDoneActing) {
                setupNextActionableUnit();
            }
        } else if (state == BattleSystemState.VICTORY) {
            //TODO
        } else if (state == BattleSystemState.DEFEAT) {
            //TODO
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
            refreshDataForNextActionableUnit();
        }
    }

    private void setupNextActionableUnit() {
        bool allEnemiesAreGone = enemyBattleUnits
            .All(enemy => enemy.IsEnemy && !enemy.canAct());
        bool allPlayersCannotAct = playerBattleUnits
            .All(playerUnit => !playerUnit.canAct());
        if (allEnemiesAreGone) {
            Debug.Log("Victory!");
            List<EnemyBattleUnit> enemiesYieldingRewards = enemyBattleUnits
                .Where(enemy => enemy.yieldsRewards())
                .ToList();
            int gil = 0;
            int experience = 0;
            enemiesYieldingRewards.ForEach(enemy => {
                gil += enemy.Gil;
                experience += enemy.Experience;
            });
            Debug.Log($"Gil: {gil}");
            Debug.Log($"Experience: {experience}");
            state = BattleSystemState.VICTORY;
            return;
        } else if (allPlayersCannotAct) {
            Debug.Log("Defeat!");
            state = BattleSystemState.DEFEAT;
            return;
        }

        actionQueueBattleUnits = actionQueueBattleUnits
            .Where(unit => !unit.IsEnemy || (unit.IsEnemy && unit.canAct()))
            .ToList();

        do {
            actionQueueBattleUnits.RemoveAt(0);
            actionQueueBattleUnits.Add(currentBattleUnit);
            currentBattleUnit = actionQueueBattleUnits[0];
        } while (!currentBattleUnit.canAct());

        refreshDataForNextActionableUnit();
    }

    private void refreshDataForNextActionableUnit() {
        battleComponents.ActionQueue.updateContent(actionQueueBattleUnits
            .Where(unit => unit.canAct())
            .ToList()
        );

        initializeBattleContext();
        currentBattleUnit.prepareToAct(battleContext);
    }

    private void initializeBattleContext() {
        List<EnemyBattleUnit> activeEnemyBattleUnits = enemyBattleUnits
            .Where(enemy => enemy.canAct())
            .ToList();

        battleContext.initialize(playerBattleUnits, activeEnemyBattleUnits);
    }

    private IEnumerator performStart() {
        state = BattleSystemState.BUSY;

        List<BattleUnit> activeUnits = playerBattleUnits
            .Cast<BattleUnit>()
            .Concat(enemyBattleUnits)
            .ToList();
        BattleUnit synchronizingBattleUnit = activeUnits[0];
        activeUnits.RemoveAt(0);

        activeUnits.ForEach(unit => StartCoroutine(unit.enterBattle()));
        yield return synchronizingBattleUnit.enterBattle();

        initializeActionQueue();
        state = BattleSystemState.PROCESSING_UNITS;
    }
}

public enum BattleSystemState {
    START,
    PROCESSING_UNITS,
    VICTORY,
    DEFEAT,
    BUSY
}
