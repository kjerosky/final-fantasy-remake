using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUnit : Unit {

    private PlayerUnitBase unitBase;
    private int currentHp;
    private int maxHp;

    public int CurrentHp => currentHp;
    public int MaxHp => maxHp;
    public Sprite BattleIdleSprite => unitBase.BattleSpriteStanding;
    public List<BattleMenuCommand> BattleMenuCommands => unitBase.BattleMenuCommands;

    public PlayerUnit(PlayerUnitBase unitBase) {
        this.unitBase = unitBase;
        maxHp = unitBase.Hp;
        currentHp = maxHp;
    }

    public int takeDamage(BattleUnit attackingUnit) {
        //TODO REPLACE THIS WITH PROPERLY DETERMINED DAMAGE
        int TEMP_damageTaken = 5;

        currentHp = Mathf.Max(0, currentHp - TEMP_damageTaken);

        return TEMP_damageTaken;
    }

    public IEnumerator die(float transitionSeconds, Image unitImage, HpInfo unitHpInfo) {
        //TODO
        yield return null;
    }
}
