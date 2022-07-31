using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class VictoryComponents : MonoBehaviour {

    [SerializeField] BattleSummaryWindow summaryWindow;
    [SerializeField] float windowSlideSeconds;
    [SerializeField] float summaryWindowOffscreenOffsetY;

    public event Action OnEndVictoryProcess;

    private RectTransform summaryWindowRectTransform;
    private VictoryComponentsState state;

    private float summaryWindowStartY;

    void Start() {
        state = VictoryComponentsState.BUSY;

        summaryWindowRectTransform = summaryWindow.GetComponent<RectTransform>();
        summaryWindowStartY = summaryWindowRectTransform.anchoredPosition.y;
    }

    void Update() {
        if (state == VictoryComponentsState.WAIT_TO_DISMISS_SUMMARY) {
            handleWaitToDismissSummary();
        }
    }

    public void setup(List<PlayerBattleUnit> playerBattleUnits) {
        summaryWindow.setup(playerBattleUnits);
    }

    private void handleWaitToDismissSummary() {
        if (Input.GetKeyDown(KeyCode.Return)) {
            StartCoroutine(dismissSummaryWindow());
        }
    }

    public IEnumerator startVictoryProcess(int gil, int experiencePerUnit, List<LevelUpResult> levelUpResults) {
        summaryWindow.gameObject.SetActive(true);
        summaryWindow.setMainAwards(gil, experiencePerUnit);
        
        Vector2 offscreenSummaryWindowPosition = new Vector2(
            summaryWindowRectTransform.anchoredPosition.x, summaryWindowOffscreenOffsetY);
        summaryWindowRectTransform.anchoredPosition = offscreenSummaryWindowPosition;

        yield return summaryWindowRectTransform
            .DOAnchorPosY(summaryWindowStartY, windowSlideSeconds)
            .SetEase(Ease.OutQuad)
            .WaitForCompletion();


        StartCoroutine(summaryWindow.animateExperienceCounting(experiencePerUnit, levelUpResults));

        state = VictoryComponentsState.WAIT_TO_DISMISS_SUMMARY;
    }

    private IEnumerator dismissSummaryWindow() {
        state = VictoryComponentsState.BUSY;

        yield return summaryWindowRectTransform
            .DOAnchorPosY(-summaryWindowOffscreenOffsetY, windowSlideSeconds / 2)
            .SetEase(Ease.InQuad)
            .WaitForCompletion();

        summaryWindow.gameObject.SetActive(false);

        OnEndVictoryProcess?.Invoke();
    }
}

public enum VictoryComponentsState {
    BUSY,
    WAIT_TO_DISMISS_SUMMARY
}
