using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SummaryWindowUnitInfo : MonoBehaviour {

    [SerializeField] Image icon;
    [SerializeField] Text nameText;
    [SerializeField] Image xpBarFill;
    [SerializeField] Text levelUpText;
    [SerializeField] Text toNextLevelText;

    private PlayerUnit unit;

    public void setup(PlayerBattleUnit playerBattleUnit) {
        unit = (PlayerUnit)(playerBattleUnit.Unit);

        icon.sprite = unit.UnitActionQueueSprite;
        nameText.text = unit.Name;
        levelUpText.enabled = false;

        int level = unit.Level;
        int experienceNeededForNextLevel = LevelUpCalculator.calculateExperienceNeededForNextLevel(level);
        toNextLevelText.text = experienceNeededForNextLevel + "";

        float experienceRatio = (float)unit.Experience / experienceNeededForNextLevel;
        xpBarFill.rectTransform.localScale = new Vector3(experienceRatio, 1f, 1f);
    }

    public IEnumerator animateExperienceCounting(int experience, LevelUpResult levelUpResult) {
        //TODO
        yield return null;
    }
}
