using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleSummaryWindow : MonoBehaviour {

    [SerializeField] SummaryWindowUnitInfo unitInfo1;
    [SerializeField] SummaryWindowUnitInfo unitInfo2;
    [SerializeField] SummaryWindowUnitInfo unitInfo3;
    [SerializeField] SummaryWindowUnitInfo unitInfo4;
    [SerializeField] Text gilAwardedText;
    [SerializeField] Text experienceAwardedText;

    public void setup(List<PlayerBattleUnit> playerBattleUnits) {
        unitInfo1.setup(playerBattleUnits[0]);
        unitInfo2.setup(playerBattleUnits[1]);
        unitInfo3.setup(playerBattleUnits[2]);
        unitInfo4.setup(playerBattleUnits[3]);
    }

    public void setMainAwards(int gil, int experiencePerUnit) {
        gilAwardedText.text = gil + " Gil";
        experienceAwardedText.text = experiencePerUnit + " Exp";
    }

    public IEnumerator animateExperienceCounting(int experiencePerUnit, List<LevelUpResult> levelUpResults) {
        yield return new WaitForSeconds(0.25f);

        StartCoroutine(unitInfo1.animateExperienceCounting(experiencePerUnit, levelUpResults[0]));
        StartCoroutine(unitInfo2.animateExperienceCounting(experiencePerUnit, levelUpResults[1]));
        StartCoroutine(unitInfo3.animateExperienceCounting(experiencePerUnit, levelUpResults[2]));
        StartCoroutine(unitInfo4.animateExperienceCounting(experiencePerUnit, levelUpResults[3]));
    }
}
