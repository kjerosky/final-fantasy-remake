using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface BattleUnit {
    public bool canAct();
    public void prepareToAct(BattleContext battleContext);
    public bool act();

    public bool IsEnemy { get; }
    public int TeamMemberIndex { get; }
    public Sprite UnitActionQueueSprite { get; }
    public int CurrentHp { get; }

    public IEnumerator takePhysicalDamage(BattleUnit attackingUnit);
    public void setSelected(bool isSelected);
}
