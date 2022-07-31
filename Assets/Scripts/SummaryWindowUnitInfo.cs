using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class SummaryWindowUnitInfo : MonoBehaviour {

    [SerializeField] Image icon;
    [SerializeField] Text nameText;
    [SerializeField] Image xpBarFill;
    [SerializeField] Text levelUpText;
    [SerializeField] Text toNextLevelText;
    [SerializeField] float experienceCountingSeconds;

    private PlayerUnit unit;
    private Sequence levelUpSequence;

    public void setup(PlayerBattleUnit playerBattleUnit) {
        unit = (PlayerUnit)(playerBattleUnit.Unit);

        icon.sprite = unit.UnitActionQueueSprite;
        nameText.text = unit.Name;
        levelUpText.rectTransform.localScale = Vector3.zero;

        int level = unit.Level;
        int experienceNeededForNextLevel = LevelUpCalculator.calculateExperienceNeededForNextLevel(level);
        toNextLevelText.text = (experienceNeededForNextLevel - unit.Experience) + "";

        float experienceRatio = (float)unit.Experience / experienceNeededForNextLevel;
        xpBarFill.rectTransform.localScale = new Vector3(experienceRatio, 1f, 1f);
    }

    public IEnumerator animateExperienceCounting(int experience, LevelUpResult levelUpResult) {
        if (unit.Level >= 50) {
            yield break;
        }

        float experienceCountingRate = experience / experienceCountingSeconds;
        int targetLevel = levelUpResult.NewLevel;
        float targetExperienceForTargetLevel = levelUpResult.NewLeftoverExperience;

        int currentLevel = levelUpResult.OldLevel;
        float currentExperience = levelUpResult.OldLeftoverExperience;
        int experienceNeededForNextLevel = LevelUpCalculator.calculateExperienceNeededForNextLevel(currentLevel);
        while (currentLevel != targetLevel || currentExperience != targetExperienceForTargetLevel) {
            if (currentLevel == targetLevel) {
                currentExperience = Mathf.MoveTowards(
                    currentExperience, targetExperienceForTargetLevel, experienceCountingRate * Time.deltaTime);
            } else {
                currentExperience = Mathf.MoveTowards(
                    currentExperience, experienceNeededForNextLevel, experienceCountingRate * Time.deltaTime);
                if (currentExperience == experienceNeededForNextLevel) {
                    currentLevel++;
                    currentExperience = 0f;
                    experienceNeededForNextLevel = LevelUpCalculator.calculateExperienceNeededForNextLevel(currentLevel);
                    animateLevelUpText();
                }
            }

            float experienceRatio = (float)currentExperience / experienceNeededForNextLevel;
            xpBarFill.rectTransform.localScale = new Vector3(experienceRatio, 1f, 1f);

            int toNextLevelAmount = Mathf.RoundToInt(experienceNeededForNextLevel - currentExperience);
            toNextLevelText.text = toNextLevelAmount + "";

            yield return null;
        }
    }

    private void animateLevelUpText() {
        levelUpText.rectTransform.localScale = Vector3.zero;

        if (levelUpSequence == null) {
            levelUpSequence = DOTween.Sequence()
                .Append(levelUpText.rectTransform
                    .DOScale(new Vector3(1f, 1f, 1f), 0.25f)
                    .SetEase(Ease.OutBack))
                .AppendInterval(0.75f)
                .Append(levelUpText.rectTransform
                    .DOScale(Vector3.zero, 0.25f)
                    .SetEase(Ease.InBack))
                .SetAutoKill(false);
        } else {
            levelUpSequence.Restart();
        }
    }

    void OnDisable() {
        if (levelUpSequence != null) {
            levelUpSequence.Kill();
        }
    }
}
