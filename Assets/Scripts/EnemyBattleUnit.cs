using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBattleUnit : MonoBehaviour, BattleUnit {

    private EnemyUnit enemyUnit;
    private int teamMemberIndex;

    private bool isDoneActing;

    public void setup(EnemyUnit enemyUnit, int teamMemberIndex) {
        this.enemyUnit = enemyUnit;
        this.teamMemberIndex = teamMemberIndex;
    }

    public bool canAct() {
        return true;
    }

    public void prepareToAct() {
        isDoneActing = false;

        Debug.Log($"{name} is acting.  Thinking...");
        StartCoroutine(TEMP_waitSomeTime());
    }

    public bool act() {
        return isDoneActing;
    }

    private IEnumerator TEMP_waitSomeTime() {
        yield return new WaitForSeconds(1f);

        isDoneActing = true;
    }
}
