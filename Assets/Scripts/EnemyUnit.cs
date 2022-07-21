using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class EnemyUnit : Unit {

    private EnemyUnitBase unitBase;
    private int currentHp;
    private int maxHp;

    public int CurrentHp => currentHp;
    public int MaxHp => maxHp;
    public Sprite BattleIdleSprite => unitBase.BattleSprite;
    public List<BattleMenuCommand> BattleMenuCommands => null;

    public EnemyUnit(EnemyUnitBase unitBase) {
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
        unitHpInfo.gameObject.SetActive(false);

        yield return unitImage
            .DOFade(0f, transitionSeconds)
            .SetEase(Ease.Linear)
            .WaitForCompletion();
    }
}
