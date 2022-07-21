using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class BattleUnit : MonoBehaviour {

    [SerializeField] HpInfo hpInfo;
    [SerializeField] Image unitImage;
    [SerializeField] SelectionCursor selectionCursor;
    [SerializeField] RectTransform damageNumbers;
    [SerializeField] float damageNumbersFirstPopOffsetY;
    [SerializeField] float damageNumbersSecondPopOffsetY;
    [SerializeField] float takeDamageSeconds;

    private Unit unit;
    private float damageNumbersStartAnchorPosY;

    public int CurrentHp => unit.CurrentHp;
    public List<BattleMenuCommand> BattleMenuCommands => unit.BattleMenuCommands;


    void Awake() {
        damageNumbersStartAnchorPosY = damageNumbers.anchoredPosition.y;
    }

    public void setup(Unit unit) {
        this.unit = unit;

        hpInfo.setHp(unit.CurrentHp, unit.MaxHp);

        unitImage.sprite = unit.BattleIdleSprite;

        setSelected(false);
    }

    public void setSelected(bool isSelected) {
        selectionCursor.setShowing(isSelected);
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
