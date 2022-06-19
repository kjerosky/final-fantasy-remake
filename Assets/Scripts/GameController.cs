using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

    private enum GameState {
        ROAMING,
        DIALOG,
        SCENE_TRANSITION,
        BATTLE_TRANSITION,
        BATTLE
    };

    public Player player;

    private DialogManager dialogManager;
    private TransitionManager transitionManager;
    private BattleTransitionManager battleTransitionManager;
    private BattleManager battleManager;

    private GameState state;

    void Start() {
        dialogManager = GetComponent<DialogManager>();
        transitionManager = GetComponent<TransitionManager>();
        battleTransitionManager = GetComponent<BattleTransitionManager>();
        battleManager = GetComponent<BattleManager>();

        state = GameState.SCENE_TRANSITION;

        dialogManager.OnShowDialog += () => {
            state = GameState.DIALOG;
        };

        dialogManager.OnDialogClosed += () => {
            state = GameState.ROAMING;
        };

        transitionManager.OnStartTransition += () => {
            state = GameState.SCENE_TRANSITION;
        };

        transitionManager.OnEndTransition += () => {
            state = GameState.ROAMING;
        };

        battleTransitionManager.OnStartTransitionIntoBattle += () => {
            state = GameState.BATTLE_TRANSITION;
        };

        battleTransitionManager.OnEndTransitionIntoBattle += () => {
            state = GameState.BATTLE;
        };

        battleTransitionManager.OnStartTransitionOutOfBattle += () => {
            state = GameState.BATTLE_TRANSITION;
        };

        battleTransitionManager.OnEndTransitionOutOfBattle += () => {
            state = GameState.ROAMING;
        };
    }

    void Update() {
        if (state == GameState.ROAMING) {
            player.handleUpdate();
        } else if (state == GameState.DIALOG) {
            dialogManager.handleUpdate();
        } else if (state == GameState.BATTLE) {
            battleManager.handleUpdate();
        }
    }
}
