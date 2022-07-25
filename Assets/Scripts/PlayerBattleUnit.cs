using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBattleUnit : MonoBehaviour, BattleUnit {

    private PlayerUnit playerUnit;
    private int teamMemberIndex;

    private bool isDoneActing;

    public void setup(PlayerUnit playerUnit, int teamMemberIndex) {
        this.playerUnit = playerUnit;
        this.teamMemberIndex = teamMemberIndex;
    }

    public bool canAct() {
        //TODO ACCOUNT FOR OTHER STATUSES HERE TOO
        return playerUnit.CurrentHp > 0;
    }

    public void prepareToAct() {
        isDoneActing = false;

        Debug.Log($"{name} is acting.  Waiting for input...");
    }

    public bool act() {
        isDoneActing = Input.GetKeyDown(KeyCode.Return);

        return isDoneActing;
    }
}
