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
    [SerializeField] EnemyBattleUnit enemyUnitSmall3;
    [SerializeField] EnemyBattleUnit enemyUnitSmall4;
    [SerializeField] EnemyBattleUnit enemyUnitSmall5;
    [SerializeField] EnemyBattleUnit enemyUnitSmall6;

    //TODO REMOVE THESE TEMPORARY FIELDS
    [SerializeField] PlayerUnitBase playerUnitBase1;
    [SerializeField] PlayerUnitBase playerUnitBase2;
    [SerializeField] PlayerUnitBase playerUnitBase3;
    [SerializeField] PlayerUnitBase playerUnitBase4;
    [SerializeField] EnemyUnitBase enemyUnitBase1;
    [SerializeField] EnemyUnitBase enemyUnitBase2;
    [SerializeField] EnemyUnitBase enemyUnitBase3;
    [SerializeField] EnemyUnitBase enemyUnitBase4;
    [SerializeField] EnemyUnitBase enemyUnitBase5;
    [SerializeField] EnemyUnitBase enemyUnitBase6;
    [SerializeField] Weapon playerWeapon1;
    [SerializeField] Weapon playerWeapon2;
    [SerializeField] Weapon playerWeapon3;
    [SerializeField] Weapon playerWeapon4;

    private UnitActionQueue actionQueue;
    private BattleMenu battleMenu;

    private BattleSystemState state;
    private List<BattleUnit> actionQueueBattleUnits;
    private BattleUnit currentBattleUnit;

    private BattleContext battleContext;
    private List<PlayerBattleUnit> playerBattleUnits;
    private List<EnemyBattleUnit> enemyBattleUnits;

    void Start() {
        actionQueue = BattleComponents.Instance.ActionQueue;
        battleMenu = BattleComponents.Instance.BattleMenu;

        PlayerUnit unit1 = new PlayerUnit(playerUnitBase1, "Abraham");
        PlayerUnit unit2 = new PlayerUnit(playerUnitBase2, "Bobby");
        PlayerUnit unit3 = new PlayerUnit(playerUnitBase3, "Carly");
        PlayerUnit unit4 = new PlayerUnit(playerUnitBase4, "Diana");
        unit1.Weapon = playerWeapon1;
        unit2.Weapon = playerWeapon2;
        unit3.Weapon = playerWeapon3;
        unit4.Weapon = playerWeapon4;
        playerUnit1.setup(unit1, 0, playerWeapon1);
        playerUnit2.setup(unit2, 1, playerWeapon2);
        playerUnit3.setup(unit3, 2, playerWeapon3);
        playerUnit4.setup(unit4, 3, playerWeapon4);

        enemyUnitSmall1.setup(new EnemyUnit(enemyUnitBase1), 0);
        enemyUnitSmall2.setup(new EnemyUnit(enemyUnitBase2), 1);
        enemyUnitSmall3.setup(new EnemyUnit(enemyUnitBase3), 2);
        enemyUnitSmall4.setup(new EnemyUnit(enemyUnitBase4), 3);
        enemyUnitSmall5.setup(new EnemyUnit(enemyUnitBase5), 4);
        enemyUnitSmall6.setup(new EnemyUnit(enemyUnitBase6), 5);

        playerBattleUnits = new List<PlayerBattleUnit>() {
            playerUnit1,
            playerUnit2,
            playerUnit3,
            playerUnit4
        };

        enemyBattleUnits = new List<EnemyBattleUnit>() {
            enemyUnitSmall1,
            enemyUnitSmall2,
            enemyUnitSmall3,
            enemyUnitSmall4,
            enemyUnitSmall5,
            enemyUnitSmall6
        };

        battleContext = new BattleContext();

        actionQueue.gameObject.SetActive(false);
        battleMenu.setShowingCommandsMenu(false);

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
            playerUnit2,
            playerUnit3,
            playerUnit4,
            enemyUnitSmall1,
            enemyUnitSmall2,
            enemyUnitSmall3,
            enemyUnitSmall4,
            enemyUnitSmall5,
            enemyUnitSmall6
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

        do {
            actionQueueBattleUnits.RemoveAt(0);
            actionQueueBattleUnits.Add(currentBattleUnit);
            currentBattleUnit = actionQueueBattleUnits[0];
        } while (!currentBattleUnit.canAct());

        refreshDataForNextActionableUnit();
    }

    private void refreshDataForNextActionableUnit() {
        actionQueue.updateContent(actionQueueBattleUnits
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
