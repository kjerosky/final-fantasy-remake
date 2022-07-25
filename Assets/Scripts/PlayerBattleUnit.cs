using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PlayerBattleUnit : MonoBehaviour, BattleUnit {

    [SerializeField] Image unitImage;
    [SerializeField] Text nameText;
    [SerializeField] HpInfo hpInfo;
    [SerializeField] Text damageNumbersText;
    [SerializeField] float damageKnockbackDistanceX;
    [SerializeField] float damageKnockbackTotalSeconds;
    [SerializeField] float damageNumbersFirstPopOffsetY;
    [SerializeField] float damageNumbersSecondPopOffsetY;
    [SerializeField] float damageTakenTotalSeconds;

    private PlayerUnit playerUnit;
    private int teamMemberIndex;

    private bool isDoneActing;

    private RectTransform unitImageRectTransform;
    private float unitImageStartX;
    private RectTransform damageNumbersRectTransform;
    private float damageNumbersStartY;

    public int CurrentHp => playerUnit.CurrentHp;

    public void setup(PlayerUnit playerUnit, int teamMemberIndex) {
        this.playerUnit = playerUnit;
        this.teamMemberIndex = teamMemberIndex;

        unitImage.sprite = playerUnit.BattleSpriteStanding;

        nameText.text = playerUnit.Name;

        hpInfo.setHp(playerUnit.CurrentHp, playerUnit.MaxHp);

        unitImageRectTransform = unitImage.GetComponent<RectTransform>();
        unitImageStartX = unitImageRectTransform.anchoredPosition.x;
        damageNumbersRectTransform = damageNumbersText.GetComponent<RectTransform>();
        damageNumbersStartY = damageNumbersRectTransform.anchoredPosition.y;
    }

    public bool canAct() {
        //TODO ACCOUNT FOR OTHER STATUSES HERE TOO
        return playerUnit.CurrentHp > 0;
    }

    public void prepareToAct(BattleContext battleContext) {
        isDoneActing = false;

        Debug.Log($"{name} is acting.  Waiting for input...");
    }

    public bool act(BattleContext battleContext) {
        isDoneActing = Input.GetKeyDown(KeyCode.Return);

        return isDoneActing;
    }

    public IEnumerator takePhysicalDamage(BattleUnit attackingUnit) {
        int damage = determinePhysicalDamage(attackingUnit);
        playerUnit.takeDamage(damage);

        if (damage > 0) {
            yield return animateReactionToDamage();
        }

        yield return animateDamageTaken(damage);
    }

    private int determinePhysicalDamage(BattleUnit attackingUnit) {
        //TODO REPLACE THIS WITH PROPERLY DETERMINED DAMAGE
        int TEMP_damageTaken = 5;
        return TEMP_damageTaken;
    }

    private IEnumerator animateReactionToDamage() {
        float farRightX = unitImageStartX + damageKnockbackDistanceX;
        float closerRightX = unitImageStartX + damageKnockbackDistanceX / 2;

        Sequence shakeUnit = DOTween.Sequence()
            .Append(unitImageRectTransform
                .DOAnchorPosX(farRightX, damageKnockbackTotalSeconds / 4)
                .SetEase(Ease.Linear)
            ).Append(unitImageRectTransform
                .DOAnchorPosX(unitImageStartX, damageKnockbackTotalSeconds / 4)
                .SetEase(Ease.Linear)
            ).Append(unitImageRectTransform
                .DOAnchorPosX(closerRightX, damageKnockbackTotalSeconds / 4)
                .SetEase(Ease.Linear)
            ).Append(unitImageRectTransform
                .DOAnchorPosX(unitImageStartX, damageKnockbackTotalSeconds / 4)
                .SetEase(Ease.Linear)
            );

        yield return shakeUnit.WaitForCompletion();
    }

    private IEnumerator animateDamageTaken(int damage) {
        damageNumbersText.gameObject.SetActive(true);
        damageNumbersText.text = damage + "";

        Sequence damageNumbersBouncing = DOTween.Sequence()
            .Append(damageNumbersRectTransform
                .DOAnchorPosY(damageNumbersStartY + damageNumbersFirstPopOffsetY, damageTakenTotalSeconds / 5)
                .SetEase(Ease.OutQuad)
            ).Append(damageNumbersRectTransform
                .DOAnchorPosY(damageNumbersStartY, damageTakenTotalSeconds / 5)
                .SetEase(Ease.InQuad)
            ).Append(damageNumbersRectTransform
                .DOAnchorPosY(damageNumbersStartY + damageNumbersSecondPopOffsetY, damageTakenTotalSeconds / 8)
                .SetEase(Ease.OutQuad)
            ).Append(damageNumbersRectTransform
                .DOAnchorPosY(damageNumbersStartY, damageTakenTotalSeconds / 8)
                .SetEase(Ease.InQuad)
            );
        damageNumbersBouncing.Play();

        yield return hpInfo.setHpSmooth(playerUnit.CurrentHp, playerUnit.MaxHp, damageTakenTotalSeconds);

        damageNumbersText.gameObject.SetActive(false);
    }
}
