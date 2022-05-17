using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

    private enum GameState {
        ROAMING,
        DIALOG,
        SCENE_TRANSITION
    };

    public Player player;

    private DialogManager dialogManager;
    private TransitionManager transitionManager;

    private GameState state;

    void Start() {
        dialogManager = GetComponent<DialogManager>();
        transitionManager = GetComponent<TransitionManager>();

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
    }

    void Update() {
        if (state == GameState.ROAMING) {
            player.handleUpdate();
        } else if (state == GameState.DIALOG) {
            dialogManager.handleUpdate();
        }
    }
}
