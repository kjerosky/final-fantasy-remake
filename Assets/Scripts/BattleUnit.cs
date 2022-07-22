using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class BattleUnit : MonoBehaviour {

    [SerializeField] HpInfo hpInfo;
    [SerializeField] Image unitImage;
    [SerializeField] SelectionCursor selectionCursor;
    [SerializeField] Text nameText;
    [SerializeField] RectTransform damageNumbers;
    [SerializeField] float damageNumbersFirstPopOffsetY;
    [SerializeField] float damageNumbersSecondPopOffsetY;
    [SerializeField] float takeDamageSeconds;

    private Unit unit;
    private float damageNumbersStartAnchorPosY;
    private bool isEnemyUnit;
    private int teamMemberIndex;

    public int CurrentHp => unit.CurrentHp;
    public Sprite UnitActionQueueSprite => unit.UnitActionQueueSprite;
    public List<BattleMenuCommand> BattleMenuCommands => unit.BattleMenuCommands;
    public bool IsEnemyUnit => isEnemyUnit;
    public int TeamMemberIndex {
        get => teamMemberIndex;
        set => teamMemberIndex = value;
    }


    void Awake() {
        damageNumbersStartAnchorPosY = damageNumbers.anchoredPosition.y;
    }

    public void setup(Unit unit, bool isEnemyUnit) {
        this.unit = unit;
        this.isEnemyUnit = isEnemyUnit;

        if (nameText != null) {
            nameText.text = unit.Name;
        }

        hpInfo.setHp(unit.CurrentHp, unit.MaxHp);

        unitImage.sprite = unit.BattleIdleSprite;

        setSelected(false);
    }

    public void setSelected(bool isSelected) {
        selectionCursor.setShowing(isSelected);
    }

    public IEnumerator beforeAttacking() {
        yield return unit.beforeDealingDamage(unitImage);
    }

    public IEnumerator takeDamagePhysical(BattleUnit attackingUnit) {
        int damageTaken = unit.takeDamage(attackingUnit);

        damageNumbers.gameObject.SetActive(true);
        damageNumbers.GetComponent<Text>().text = damageTaken + "";

        Sequence damageNumbersBouncing = DOTween.Sequence()
            .Append(damageNumbers
                .DOAnchorPosY(damageNumbersStartAnchorPosY + damageNumbersFirstPopOffsetY, takeDamageSeconds / 5)
                .SetEase(Ease.OutQuad)
            ).Append(damageNumbers
                .DOAnchorPosY(damageNumbersStartAnchorPosY, takeDamageSeconds / 5)
                .SetEase(Ease.InQuad)
            ).Append(damageNumbers
                .DOAnchorPosY(damageNumbersStartAnchorPosY + damageNumbersSecondPopOffsetY, takeDamageSeconds / 8)
                .SetEase(Ease.OutQuad)
            ).Append(damageNumbers
                .DOAnchorPosY(damageNumbersStartAnchorPosY, takeDamageSeconds / 8)
                .SetEase(Ease.InQuad)
            );
        damageNumbersBouncing.Play();

        yield return hpInfo.setHpSmooth(unit.CurrentHp, unit.MaxHp, takeDamageSeconds);

        damageNumbers.gameObject.SetActive(false);
    }

    public IEnumerator die(float transitionSeconds) {
        yield return unit.die(transitionSeconds, unitImage, hpInfo);
    }

}
