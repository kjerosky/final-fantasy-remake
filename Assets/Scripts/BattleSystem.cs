using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BattleSystem : MonoBehaviour {

    [SerializeField] BattleUnit playerUnit1;
    [SerializeField] BattleUnit enemyUnitSmall1;
    [SerializeField] BattleUnit enemyUnitSmall2;
    [SerializeField] BattleMenu battleMenu;
    [SerializeField] float enemyDeathFadeOutSeconds;

    //TODO REMOVE THESE TEMPORARY FIELDS
    [SerializeField] PlayerUnitBase playerUnitBase1;
    [SerializeField] EnemyUnitBase enemyUnitBase1;
    [SerializeField] EnemyUnitBase enemyUnitBase2;

    private BattleState state;

    private int currentMenuCommandsCount;
    private int currentSelectedMenuCommandIndex;
    private PlayerUnitCommand chosenCommand;

    private List<BattleUnit> activeEnemyBattleUnits;
    private int currentSelectedEnemyBattleUnitIndex;

    private List<BattleUnit> unitActionQueue;
    private BattleUnit currentBattleUnit;

    void Start() {
        state = BattleState.START;

        playerUnit1.setup(new PlayerUnit(playerUnitBase1, "Abraham"), false);
        playerUnit1.TeamMemberIndex = 0;

        enemyUnitSmall1.setup(new EnemyUnit(enemyUnitBase1), true);
        enemyUnitSmall2.setup(new EnemyUnit(enemyUnitBase2), true);
        enemyUnitSmall1.TeamMemberIndex = 0;
        enemyUnitSmall2.TeamMemberIndex = 1;

        unitActionQueue = new List<BattleUnit>() {
            playerUnit1,
            enemyUnitSmall1,
            enemyUnitSmall2
        };

        activateNextUnit();
    }

    void Update() {
        if (state == BattleState.PLAYER_SELECT_ACTION) {
            handlePlayerSelectAction();
        } else if (state == BattleState.PLAYER_SELECT_SINGLE_TARGET) {
            handlePlayerSelectSingleTarget();
        } else if (state == BattleState.ENEMY_ACTION) {
            handleEnemyAction();
        }
    }

    private void activateNextUnit() {
        activeEnemyBattleUnits = new List<BattleUnit>() {
            enemyUnitSmall1,
            enemyUnitSmall2
        }
            .Where(unit => unit.gameObject.activeSelf)
            .ToList();
        if (activeEnemyBattleUnits.Count <= 0) {
            state = BattleState.PLAYER_WON;
            return;
        }

        List<BattleUnit> alivePlayerUnits = new List<BattleUnit>() {
            playerUnit1
        }
            .Where(unit => unit.CurrentHp > 0)
            .ToList();
        if (alivePlayerUnits.Count <= 0) {
            state = BattleState.PLAYER_LOST;
            return;
        }

        if (currentBattleUnit != null) {
            unitActionQueue.RemoveAt(0);
            unitActionQueue.Add(currentBattleUnit);
        }
        currentBattleUnit = unitActionQueue[0];

        if (currentBattleUnit.IsEnemyUnit) {
            state = BattleState.ENEMY_ACTION;
            return;
        }

        List<BattleMenuCommand> menuCommands = currentBattleUnit.BattleMenuCommands;
        battleMenu.initializeCommands(menuCommands);
        currentMenuCommandsCount = menuCommands.Count;

        currentSelectedMenuCommandIndex = 0;
        battleMenu.setSelectedCommand(currentSelectedMenuCommandIndex);
        battleMenu.setShowingCommandsMenu(true);

        state = BattleState.PLAYER_SELECT_ACTION;
    }

    private void preparePlayerSelectSingleTarget() {
        currentSelectedEnemyBattleUnitIndex = 0;

        state = BattleState.PLAYER_SELECT_SINGLE_TARGET;
    }

    private void handlePlayerSelectAction() {
        if (Input.GetKeyDown(KeyCode.W)) {
            currentSelectedMenuCommandIndex--;
            if (currentSelectedMenuCommandIndex < 0) {
                currentSelectedMenuCommandIndex = currentMenuCommandsCount - 1;
            }
        } else if (Input.GetKeyDown(KeyCode.S)) {
            currentSelectedMenuCommandIndex++;
            if (currentSelectedMenuCommandIndex >= currentMenuCommandsCount) {
                currentSelectedMenuCommandIndex = 0;
            }
        }

        battleMenu.setSelectedCommand(currentSelectedMenuCommandIndex);

        if (Input.GetKeyDown(KeyCode.Return)) {
            chosenCommand = currentBattleUnit.BattleMenuCommands[currentSelectedMenuCommandIndex].Command;

            if (chosenCommand == PlayerUnitCommand.ATTACK) {
                battleMenu.dimCommandCursor(currentSelectedMenuCommandIndex);
                preparePlayerSelectSingleTarget();
            }
        }
    }

    private void handlePlayerSelectSingleTarget() {
        if (Input.GetKeyDown(KeyCode.W)) {
            currentSelectedEnemyBattleUnitIndex--;
            if (currentSelectedEnemyBattleUnitIndex < 0) {
                currentSelectedEnemyBattleUnitIndex = activeEnemyBattleUnits.Count - 1;
            }
        } else if (Input.GetKeyDown(KeyCode.S)) {
            currentSelectedEnemyBattleUnitIndex++;
            if (currentSelectedEnemyBattleUnitIndex >= activeEnemyBattleUnits.Count) {
                currentSelectedEnemyBattleUnitIndex = 0;
            }
        }

        for (int i = 0; i < activeEnemyBattleUnits.Count; i++) {
            activeEnemyBattleUnits[i].setSelected(i == currentSelectedEnemyBattleUnitIndex);
        }

        if (Input.GetKeyDown(KeyCode.Return)) {
            executeCommand();
        } else if (Input.GetKeyDown(KeyCode.Escape)) {
            if (chosenCommand == PlayerUnitCommand.ATTACK) {
                activeEnemyBattleUnits.ForEach(enemy => enemy.setSelected(false));
                battleMenu.brightenCommandCursor(currentSelectedMenuCommandIndex);
                state = BattleState.PLAYER_SELECT_ACTION;
            }
        }
    }

    private void handleEnemyAction() {
        StartCoroutine(performEnemyAttack());
    }

    private void executeCommand() {
        if (chosenCommand == PlayerUnitCommand.ATTACK) {
            StartCoroutine(performAttack());
        }
    }

    private IEnumerator performAttack() {
        state = BattleState.BUSY;

        battleMenu.brightenCommandCursor(currentSelectedMenuCommandIndex);
        battleMenu.setShowingCommandsMenu(false);
        activeEnemyBattleUnits.ForEach(enemy => enemy.setSelected(false));

        BattleUnit targetEnemy = activeEnemyBattleUnits[currentSelectedEnemyBattleUnitIndex];
        yield return targetEnemy.takeDamagePhysical(currentBattleUnit);

        if (targetEnemy.CurrentHp <= 0) {
            yield return targetEnemy.die(enemyDeathFadeOutSeconds);

            targetEnemy.gameObject.SetActive(false);
            unitActionQueue.Remove(targetEnemy);
        }

        activateNextUnit();
    }

    private IEnumerator performEnemyAttack() {
        state = BattleState.BUSY;

        yield return new WaitForSeconds(0.5f);
        yield return currentBattleUnit.beforeAttacking();

        BattleUnit targetPlayerUnit = playerUnit1;
        yield return targetPlayerUnit.takeDamagePhysical(currentBattleUnit);

        activateNextUnit();
    }

    private void handlePlayerLost() {
        //TODO
    }

    private void handlePlayerWon() {
        //TODO
    }
}

public enum BattleState {
    START,
    PLAYER_SELECT_ACTION,
    PLAYER_SELECT_SINGLE_TARGET,
    ENEMY_ACTION,
    PLAYER_LOST,
    PLAYER_WON,
    BUSY
}