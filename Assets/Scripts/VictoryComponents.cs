using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class VictoryComponents : MonoBehaviour {

    [SerializeField] BattleSummaryWindow summaryWindow;
    [SerializeField] UnitLevelUpWindow unitLevelUpWindow;
    [SerializeField] float windowSlideSeconds;
    [SerializeField] float summaryWindowOffscreenOffsetY;
    [SerializeField] float unitLevelUpWindowOffscreenOffsetY;

    public event Action OnEndVictoryProcess;

    private RectTransform summaryWindowRectTransform;
    private RectTransform unitLevelUpWindowRectTransform;
    private VictoryComponentsState state;

    private float summaryWindowStartY;
    private float unitLevelUpWindowStartY;

    private List<PlayerBattleUnit> playerBattleUnits;
    private List<LevelUpResult> levelUpResults;
    private int currentUnitIndex;

    void Start() {
        state = VictoryComponentsState.BUSY;

        summaryWindowRectTransform = summaryWindow.GetComponent<RectTransform>();
        summaryWindowStartY = summaryWindowRectTransform.anchoredPosition.y;

        unitLevelUpWindowRectTransform = unitLevelUpWindow.GetComponent<RectTransform>();
        unitLevelUpWindowStartY = unitLevelUpWindowRectTransform.anchoredPosition.y;
    }

    void Update() {
        if (state == VictoryComponentsState.WAIT_TO_DISMISS_SUMMARY) {
            handleWaitToDismissSummary();
        } else if (state == VictoryComponentsState.SHOW_LEVEL_UP_WINDOW) {
            handleShowLevelUpWindow();
        } else if (state == VictoryComponentsState.WAIT_TO_DISMISS_LEVEL_UP_WINDOW) {
            handleWaitToDismissLevelUpWindow();
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

    private void handleShowLevelUpWindow() {
        while (currentUnitIndex < levelUpResults.Count) {
            if (levelUpResults[currentUnitIndex].OldLevel != levelUpResults[currentUnitIndex].NewLevel) {
                StartCoroutine(showLevelUpWindow());
                return;
            }

            currentUnitIndex++;
        }

        state = VictoryComponentsState.BUSY;
        OnEndVictoryProcess?.Invoke();
    }

    private void handleWaitToDismissLevelUpWindow() {
        if (Input.GetKeyDown(KeyCode.Return)) {
            StartCoroutine(dismissLevelUpWindow());
        }
    }

    public IEnumerator startVictoryProcess(int gil, int experiencePerUnit, List<PlayerBattleUnit> playerBattleUnits, List<LevelUpResult> levelUpResults) {
        this.playerBattleUnits = playerBattleUnits;
        this.levelUpResults = levelUpResults;

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

        currentUnitIndex = 0;
        state = VictoryComponentsState.SHOW_LEVEL_UP_WINDOW;
    }

    private IEnumerator showLevelUpWindow() {
        state = VictoryComponentsState.BUSY;

        PlayerUnit unit = (PlayerUnit)(playerBattleUnits[currentUnitIndex].Unit);
        LevelUpResult levelUpResult = levelUpResults[currentUnitIndex];

        unitLevelUpWindow.setup(unit, levelUpResult);
        unitLevelUpWindow.gameObject.SetActive(true);

        Vector2 offscreenUnitLevelUpWindowPosition = new Vector2(
            unitLevelUpWindowRectTransform.anchoredPosition.x, unitLevelUpWindowOffscreenOffsetY);
        unitLevelUpWindowRectTransform.anchoredPosition = offscreenUnitLevelUpWindowPosition;

        yield return unitLevelUpWindowRectTransform
            .DOAnchorPosY(unitLevelUpWindowStartY, windowSlideSeconds)
            .SetEase(Ease.OutQuad)
            .WaitForCompletion();

        state = VictoryComponentsState.WAIT_TO_DISMISS_LEVEL_UP_WINDOW;
    }

    private IEnumerator dismissLevelUpWindow() {
        state = VictoryComponentsState.BUSY;

        yield return unitLevelUpWindowRectTransform
            .DOAnchorPosY(-unitLevelUpWindowOffscreenOffsetY, windowSlideSeconds / 2)
            .SetEase(Ease.InQuad)
            .WaitForCompletion();

        unitLevelUpWindow.gameObject.SetActive(false);

        currentUnitIndex++;
        state = VictoryComponentsState.SHOW_LEVEL_UP_WINDOW;
    }
}

public enum VictoryComponentsState {
    BUSY,
    WAIT_TO_DISMISS_SUMMARY,
    SHOW_LEVEL_UP_WINDOW,
    WAIT_TO_DISMISS_LEVEL_UP_WINDOW
}
