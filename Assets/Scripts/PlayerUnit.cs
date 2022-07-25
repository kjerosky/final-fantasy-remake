using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUnit {

    private PlayerUnitBase unitBase;
    private string name;
    private int currentHp;
    private int maxHp;

    public int CurrentHp => currentHp;

    public PlayerUnit(PlayerUnitBase unitBase, string name) {
        this.unitBase = unitBase;
        this.name = name;

        maxHp = unitBase.Hp;
        currentHp = maxHp;
    }
}
