using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class EnemyBattleUnit : MonoBehaviour {

    [SerializeField] EnemyUnitBase enemyUnitBase;
    [SerializeField] HpInfo hpInfo;
    [SerializeField] Image enemyUnitImage;
    [SerializeField] SelectionCursor selectionCursor;
    [SerializeField] RectTransform damageNumbers;
    [SerializeField] float damageNumbersFirstPopOffsetY;
    [SerializeField] float damageNumbersSecondPopOffsetY;
    [SerializeField] float takeDamageSeconds;

    private EnemyUnit enemyUnit;
    private float damageNumbersStartAnchorPosY;

    public int CurrentHp => enemyUnit.CurrentHp;

    void Awake() {
        damageNumbersStartAnchorPosY = damageNumbers.anchoredPosition.y;
    }

    public void setup() {
        enemyUnit = new EnemyUnit(enemyUnitBase);

        hpInfo.setHp(enemyUnit.CurrentHp, enemyUnit.MaxHp);

        enemyUnitImage.sprite = enemyUnitBase.BattleSprite;

        setSelected(false);
    }

    public void setSelected(bool isSelected) {
        selectionCursor.setShowing(isSelected);
    }

    public IEnumerator takeDamagePhysical(PlayerBattleUnit attackingPlayerUnit) {
        int damageTaken = enemyUnit.takeDamage(attackingPlayerUnit);

        damageNumbers.gameObject.SetActive(true);
        damageNumbers.GetComponent<Text>().text = damageTaken + "";

        Sequence damageNumbersBouncing = DOTween.Sequence()
            .Append(damageNumbers
                .DOAnchorPosY(damageNumbersStartAnchorPosY + damageNumbersFirstPopOffsetY, takeDamageSeconds / 4)
                .SetEase(Ease.OutQuad)
            ).Append(damageNumbers
                .DOAnchorPosY(damageNumbersStartAnchorPosY, takeDamageSeconds / 4)
                .SetEase(Ease.InQuad)
            ).Append(damageNumbers
                .DOAnchorPosY(damageNumbersStartAnchorPosY + damageNumbersSecondPopOffsetY, takeDamageSeconds / 8)
                .SetEase(Ease.OutQuad)
            ).Append(damageNumbers
                .DOAnchorPosY(damageNumbersStartAnchorPosY, takeDamageSeconds / 8)
                .SetEase(Ease.InQuad)
            );
        damageNumbersBouncing.Play();

        yield return hpInfo.setHpSmooth(enemyUnit.CurrentHp, enemyUnit.MaxHp, takeDamageSeconds);

        damageNumbers.gameObject.SetActive(false);
    }

    public IEnumerator die(float fadeOutSeconds) {
        hpInfo.gameObject.SetActive(false);

        yield return enemyUnitImage
            .DOFade(0f, fadeOutSeconds)
            .SetEase(Ease.Linear)
            .WaitForCompletion();
    }
}
