using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class EnemyBattleUnitAnimator : MonoBehaviour {

    [SerializeField] Image unitImage;
    [SerializeField] RectTransform unitImageRectTransform;
    [SerializeField] float offscreenOffsetX;
    [SerializeField] float entranceSeconds;
    [SerializeField] float takingActionFlashSeconds;
    [SerializeField] float runningAwaySeconds;
    [SerializeField] float deathTransitionSeconds;

    private float unitImageStartX;

    void Start() {
        unitImageStartX = unitImageRectTransform.anchoredPosition.x;
    }

    public IEnumerator animateBattleEntrance() {
        Vector2 battleEntranceInitialPosition = unitImageRectTransform.anchoredPosition;
        battleEntranceInitialPosition = new Vector2(
            battleEntranceInitialPosition.x + offscreenOffsetX,
            battleEntranceInitialPosition.y);
        unitImageRectTransform.anchoredPosition = battleEntranceInitialPosition;

        yield return unitImageRectTransform
            .DOAnchorPosX(unitImageStartX, entranceSeconds)
            .SetEase(Ease.OutExpo)
            .WaitForCompletion();
    }

    public IEnumerator animateTakingAction() {
        for (int i = 0; i < 2; i++) {
            yield return unitImage
                .DOColor(Color.black, takingActionFlashSeconds / 2)
                .SetEase(Ease.Linear)
                .WaitForCompletion();
            yield return unitImage
                .DOColor(Color.white, takingActionFlashSeconds / 2)
                .SetEase(Ease.Linear)
                .WaitForCompletion();
        }

    }

    public IEnumerator animateRunningAway() {
        yield return unitImageRectTransform
            .DOAnchorPosX(unitImageStartX + 30f, runningAwaySeconds / 2)
            .SetEase(Ease.OutQuad)
            .WaitForCompletion();

        yield return unitImageRectTransform
            .DOAnchorPosX(unitImageStartX + offscreenOffsetX, runningAwaySeconds / 2)
            .SetEase(Ease.InQuad)
            .WaitForCompletion();
    }

    public IEnumerator animateDeath() {
        yield return unitImage
            .DOFade(0f, deathTransitionSeconds)
            .SetEase(Ease.Linear)
            .WaitForCompletion();
    }
}
