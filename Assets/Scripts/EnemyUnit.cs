using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class EnemyUnit : Unit {

    private EnemyUnitBase unitBase;
    private int currentHp;
    private int maxHp;

    public string Name => unitBase.name;
    public int CurrentHp => currentHp;
    public int MaxHp => maxHp;
    public Sprite BattleIdleSprite => unitBase.BattleSprite;
    public Sprite UnitActionQueueSprite => null;
    public List<BattleMenuCommand> BattleMenuCommands => null;

    public EnemyUnit(EnemyUnitBase unitBase) {
        this.unitBase = unitBase;
        maxHp = unitBase.Hp;
        currentHp = maxHp;
    }

    public IEnumerator beforeDealingDamage(Image unitImage) {
        for (int i = 0; i < 2; i++) {
            yield return unitImage
                .DOColor(Color.black, 0.1f)
                .SetEase(Ease.Linear)
                .WaitForCompletion();
            yield return unitImage
                .DOColor(Color.white, 0.1f)
                .SetEase(Ease.Linear)
                .WaitForCompletion();
        }
    }

    public int takeDamage(BattleUnit attackingUnit) {
        //TODO REPLACE THIS WITH PROPERLY DETERMINED DAMAGE
        int TEMP_damageTaken = 5;

        currentHp = Mathf.Max(0, currentHp - TEMP_damageTaken);

        return TEMP_damageTaken;
    }

    public IEnumerator afterDealingDamage(Image unitImage) {
        yield return null;
    }

    public IEnumerator reactToBeingHit(Image unitImage) {
        //TODO
        yield return null;
    }

    public IEnumerator die(float transitionSeconds, Image unitImage, HpInfo unitHpInfo) {
        unitHpInfo.gameObject.SetActive(false);

        yield return unitImage
            .DOFade(0f, transitionSeconds)
            .SetEase(Ease.Linear)
            .WaitForCompletion();
    }
}
