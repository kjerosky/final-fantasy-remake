using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUnit {

    private PlayerUnitBase unitBase;
    private string name;
    private int currentHp;
    private int maxHp;

    public string Name => name;
    public int CurrentHp => currentHp;
    public int MaxHp => maxHp;
    public Sprite BattleSpriteStanding => unitBase.BattleSpriteStanding;
    public Sprite BattleSpriteWalking => unitBase.BattleSpriteWalking;
    public Sprite BattleSpriteWeaponRaised => unitBase.BattleSpriteWeaponRaised;
    public Sprite BattleSpriteKneeling => unitBase.BattleSpriteKneeling;
    public Sprite BattleSpriteDead => unitBase.BattleSpriteDead;
    public Sprite UnitActionQueueSprite => unitBase.UnitActionQueueSprite;
    public List<BattleMenuCommand> BattleMenuCommands => unitBase.BattleMenuCommands;

    public PlayerUnit(PlayerUnitBase unitBase, string name) {
        this.unitBase = unitBase;
        this.name = name;

        maxHp = unitBase.Hp;
        currentHp = maxHp;
    }

    public void takeDamage(int damage) {
        currentHp = Mathf.Max(0, currentHp - damage);
    }
}
