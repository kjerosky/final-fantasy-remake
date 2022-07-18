using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUnit {

    private PlayerUnitBase unitBase;
    private int currentHp;
    private int maxHp;

    public PlayerUnitBase Base => unitBase;
    public int MaxHp => maxHp;
    public int CurrentHp => currentHp;

    public PlayerUnit(PlayerUnitBase unitBase) {
        this.unitBase = unitBase;
        maxHp = unitBase.Hp;
        currentHp = maxHp;
    }
}
