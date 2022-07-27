using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyUnit : Unit {

    private EnemyUnitBase unitBase;
    private int currentHp;
    private int maxHp;
    private int hitMultiplier;

    public int CurrentHp => currentHp;
    public int MaxHp => maxHp;
    public int Morale => unitBase.Morale;
    public Sprite BattleSprite => unitBase.BattleSprite;
    public int Gil => unitBase.Gil;
    public int Experience => unitBase.Experience;

    public int NumberOfHits => unitBase.NumberOfHits;
    public int HitMultiplier => hitMultiplier;
    public int Accuracy => unitBase.Accuracy;
    public int Evasion => unitBase.Evasion;
    public int CriticalRate => unitBase.CriticalRate;
    public int Attack => unitBase.Attack;
    public int Defense => unitBase.Defense;

    public EnemyUnit(EnemyUnitBase unitBase) {
        this.unitBase = unitBase;

        maxHp = unitBase.Hp;
        currentHp = maxHp;
        hitMultiplier = 1;
    }

    public void takeDamage(int damage) {
        currentHp = Mathf.Max(0, currentHp - damage);
    }
}
