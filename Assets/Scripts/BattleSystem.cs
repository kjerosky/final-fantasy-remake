using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BattleSystem : MonoBehaviour {

    [SerializeField] PlayerBattleUnit playerUnit1;
    [SerializeField] EnemyBattleUnit enemyUnitSmall1;
    [SerializeField] EnemyBattleUnit enemyUnitSmall2;
    [SerializeField] BattleMenu battleMenu;

    private BattleState state;

    private PlayerBattleUnit currentPlayerBattleUnit;

    private int currentMenuCommandsCount;
    private int currentSelectedMenuCommandIndex;
    private PlayerUnitCommand chosenCommand;

    private List<EnemyBattleUnit> activeEnemyBattleUnits;
    private int currentSelectedEnemyBattleUnitIndex;

    void Start() {
        state = BattleState.START;

        playerUnit1.setup();
        enemyUnitSmall1.setup();
        enemyUnitSmall2.setup();

        activateNextUnit();
    }

    void Update() {
        if (state == BattleState.PLAYER_SELECT_ACTION) {
            handlePlayerSelectAction();
        } else if (state == BattleState.PLAYER_SELECT_SINGLE_TARGET) {
            handlePlayerSelectSingleTarget();
        }
    }

    private void activateNextUnit() {
        currentPlayerBattleUnit = playerUnit1;

        List<BattleMenuCommand> menuCommands = currentPlayerBattleUnit.BattleMenuCommands;
        battleMenu.initializeCommands(menuCommands);
        currentMenuCommandsCount = menuCommands.Count;

        currentSelectedMenuCommandIndex = 0;
        battleMenu.setSelectedCommand(currentSelectedMenuCommandIndex);

        state = BattleState.PLAYER_SELECT_ACTION;
    }

    private void preparePlayerSelectSingleTarget() {
        activeEnemyBattleUnits = new List<EnemyBattleUnit>() {
            enemyUnitSmall1,
            enemyUnitSmall2
        }
            .Where(unit => unit.gameObject.activeSelf)
            .ToList();

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
            chosenCommand = currentPlayerBattleUnit.BattleMenuCommands[currentSelectedMenuCommandIndex].Command;

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
            //TODO EXECUTE COMMAND
        } else if (Input.GetKeyDown(KeyCode.Escape)) {
            if (chosenCommand == PlayerUnitCommand.ATTACK) {
                activeEnemyBattleUnits.ForEach(enemy => enemy.setSelected(false));
                battleMenu.brightenCommandCursor(currentSelectedMenuCommandIndex);
                state = BattleState.PLAYER_SELECT_ACTION;
            }
        }
    }
}

public enum BattleState {
    START,
    PLAYER_SELECT_ACTION,
    PLAYER_SELECT_SINGLE_TARGET,
    ENEMY_ACTION,
    BUSY
}
