using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

    private enum GameState {
        ROAMING,
        DIALOG
    };

    public Player player;

    private DialogManager dialogManager;

    private GameState state;

    void Start() {
        dialogManager = GetComponent<DialogManager>();

        state = GameState.ROAMING;

        dialogManager.OnShowDialog += () => {
            state = GameState.DIALOG;
        };

        dialogManager.OnDialogClosed += () => {
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
