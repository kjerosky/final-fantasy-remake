using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleSystem : MonoBehaviour {

    [SerializeField] PlayerBattleUnit playerUnit1;
    [SerializeField] EnemyBattleUnit enemyUnitSmall1;

    void Start() {
        playerUnit1.setup();
        enemyUnitSmall1.setup();
    }
}
