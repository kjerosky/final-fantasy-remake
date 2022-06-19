using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour {

    [SerializeField] BattleTransitionManager battleTransitionManager;

    public void handleUpdate() {
        if (Input.GetKeyDown(KeyCode.B)) {
            battleTransitionManager.transitionOutOfBattle();
        }
    }
}
