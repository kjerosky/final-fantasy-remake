using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public interface Unit {
    public string Name { get; }
    public int CurrentHp { get; }
    public int MaxHp { get; }
    public Sprite BattleIdleSprite { get; }
    public Sprite UnitActionQueueSprite { get; }
    public List<BattleMenuCommand> BattleMenuCommands { get; }

    public IEnumerator enterBattle(BattleUnit myBattleUnit, float entranceSeconds);
    public IEnumerator beforeDealingDamage(BattleUnit myBattleUnit, BattleUnit targetBattleUnit);
    public int takeDamage(BattleUnit myBattleUnit, BattleUnit attackingBattleUnit);
    public IEnumerator afterDealingDamage(BattleUnit myBattleUnit, BattleUnit targetBattleUnit);
    public IEnumerator reactToBeingHit(BattleUnit myBattleUnit, BattleUnit attackingBattleUnit);
    public IEnumerator die(float transitionSeconds, Image unitImage, HpInfo unitHpInfo);
}
