using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface BattleUnit {
    public bool canAct();
    public void prepareToAct();
    public bool act();
}
