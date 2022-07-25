using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class DamageAnimator : MonoBehaviour {

    [SerializeField] RectTransform damageNumbersRectTransform;
    [SerializeField] Text damageNumbersText;
    [SerializeField] HpInfo hpInfo;
    [SerializeField] float damageNumbersFirstPopOffsetY;
    [SerializeField] float damageNumbersSecondPopOffsetY;
    [SerializeField] float damageTakenTotalSeconds;

    private float damageNumbersStartY;

    void Start() {
        damageNumbersStartY = damageNumbersRectTransform.anchoredPosition.y;
    }

    public IEnumerator animateDamage(int damage, int currentHp, int maxHp) {
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

        yield return hpInfo.setHpSmooth(currentHp, maxHp, damageTakenTotalSeconds);

        damageNumbersText.gameObject.SetActive(false);
    }
}
