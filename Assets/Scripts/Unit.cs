using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public interface Unit {
    public int CurrentHp { get; }
    public int MaxHp { get; }
    public Sprite BattleIdleSprite { get; }
    public List<BattleMenuCommand> BattleMenuCommands { get; }

    public int takeDamage(BattleUnit attackingUnit);
    public IEnumerator die(float transitionSeconds, Image unitImage, HpInfo unitHpInfo);
}
