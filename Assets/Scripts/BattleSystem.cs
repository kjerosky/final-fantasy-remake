using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleSystem : MonoBehaviour {

    [SerializeField] PlayerBattleUnit playerUnit1;
    [SerializeField] EnemyBattleUnit enemyUnitSmall1;
    [SerializeField] BattleMenu battleMenu;

    private BattleState state;

    private PlayerBattleUnit currentPlayerBattleUnit;

    private int currentMenuCommandsCount;
    private int currentSelectedMenuCommandIndex;

    void Start() {
        state = BattleState.START;

        playerUnit1.setup();
        enemyUnitSmall1.setup();

        activateNextUnit();
    }

    void Update() {
        if (state == BattleState.PLAYER_SELECT_ACTION) {
            handlePlayerAction();
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

    private void handlePlayerAction() {
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
    }
}

public enum BattleState {
    START,
    PLAYER_SELECT_ACTION,
    PLAYER_SELECT_SINGLE_TARGET,
    ENEMY_ACTION,
    BUSY
}
