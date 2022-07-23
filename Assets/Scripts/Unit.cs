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

    public IEnumerator beforeDealingDamage(Image unitImage, BattleWeapon battleWeapon, EnemyHitEffect enemyHitEffect);
    public int takeDamage(BattleUnit attackingUnit);
    public IEnumerator afterDealingDamage(Image unitImage, BattleWeapon battleWeapon);
    public IEnumerator reactToBeingHit(Image unitImage);
    public IEnumerator die(float transitionSeconds, Image unitImage, HpInfo unitHpInfo);
}
