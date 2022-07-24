using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class EnemyUnit : Unit {

    private const float BATTLE_ENTRANCE_OFFSET_X = -1000f;

    private EnemyUnitBase unitBase;
    private int currentHp;
    private int maxHp;

    public string Name => unitBase.name;
    public int CurrentHp => currentHp;
    public int MaxHp => maxHp;
    public Sprite BattleIdleSprite => unitBase.BattleSprite;
    public Sprite BattleDeadSprite => null;
    public Sprite UnitActionQueueSprite => null;
    public List<BattleMenuCommand> BattleMenuCommands => null;

    public EnemyUnit(EnemyUnitBase unitBase) {
        this.unitBase = unitBase;
        maxHp = unitBase.Hp;
        currentHp = maxHp;
    }

    public IEnumerator enterBattle(BattleUnit myBattleUnit, float entranceSeconds) {
        Image unitImage = myBattleUnit.UnitImage;
        RectTransform unitImageRectTransform = unitImage.GetComponent<RectTransform>();

        float startX = unitImageRectTransform.anchoredPosition.x;

        Vector2 battleEntranceInitialPosition = unitImageRectTransform.anchoredPosition;
        battleEntranceInitialPosition = new Vector2(
            battleEntranceInitialPosition.x + BATTLE_ENTRANCE_OFFSET_X,
            battleEntranceInitialPosition.y);
        unitImageRectTransform.anchoredPosition = battleEntranceInitialPosition;

        yield return unitImageRectTransform
            .DOAnchorPosX(startX, entranceSeconds)
            .SetEase(Ease.OutExpo)
            .WaitForCompletion();
    }

    public IEnumerator beforeDealingDamage(BattleUnit myBattleUnit, BattleUnit targetBattleUnit) {
        Image unitImage = myBattleUnit.UnitImage;

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

    public int takeDamage(BattleUnit myBattleUnit, BattleUnit attackingBattleUnit) {
        //TODO REPLACE THIS WITH PROPERLY DETERMINED DAMAGE
        int TEMP_damageTaken = 1;

        currentHp = Mathf.Max(0, currentHp - TEMP_damageTaken);

        return TEMP_damageTaken;
    }

    public IEnumerator afterDealingDamage(BattleUnit myBattleUnit, BattleUnit targetBattleUnit) {
        yield return null;
    }

    public IEnumerator reactToBeingHit(BattleUnit myBattleUnit, BattleUnit attackingBattleUnit) {
        yield return null;
    }

    public IEnumerator die(BattleUnit myBattleUnit, float transitionSeconds) {
        myBattleUnit.HpInfo.gameObject.SetActive(false);

        yield return myBattleUnit.UnitImage
            .DOFade(0f, transitionSeconds)
            .SetEase(Ease.Linear)
            .WaitForCompletion();
    }
}
