using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitLevelUpWindow : MonoBehaviour {

    [SerializeField] Color unchangedStatColor;
    [SerializeField] Color increasedStatColor;
    [SerializeField] float victoryPoseFrameSeconds;
    [SerializeField] Text nameText;
    [SerializeField] Image unitImage;
    [SerializeField] Text oldLevelText;
    [SerializeField] Text oldHpText;
    [SerializeField] Text oldStrengthText;
    [SerializeField] Text oldAgilityText;
    [SerializeField] Text oldIntelligenceText;
    [SerializeField] Text oldVitalityText;
    [SerializeField] Text oldLuckText;
    [SerializeField] Text oldBaseAccuracyText;
    [SerializeField] Text oldMagicDefenseText;
    [SerializeField] Text newLevelText;
    [SerializeField] Text newHpText;
    [SerializeField] Text newStrengthText;
    [SerializeField] Text newAgilityText;
    [SerializeField] Text newIntelligenceText;
    [SerializeField] Text newVitalityText;
    [SerializeField] Text newLuckText;
    [SerializeField] Text newBaseAccuracyText;
    [SerializeField] Text newMagicDefenseText;

    private List<Sprite> victoryPoseSprites;
    private int currentVictoryPoseSpriteIndex;
    private float timer;

    void Update() {
        timer += Time.deltaTime;
        if (timer < victoryPoseFrameSeconds) {
            return;
        }

        while (timer >= victoryPoseFrameSeconds) {
            timer -= victoryPoseFrameSeconds;
        }

        currentVictoryPoseSpriteIndex = (currentVictoryPoseSpriteIndex + 1) % victoryPoseSprites.Count;
        unitImage.sprite = victoryPoseSprites[currentVictoryPoseSpriteIndex];
    }

    public void setup(PlayerUnit unit, LevelUpResult result) {
        if (victoryPoseSprites == null) {
            victoryPoseSprites = new List<Sprite>();
        } else {
            victoryPoseSprites.Clear();
        }
        victoryPoseSprites.Add(unit.BattleSpriteStanding);
        victoryPoseSprites.Add(unit.BattleSpriteCasting);

        currentVictoryPoseSpriteIndex = 0;
        timer = 0f;

        nameText.text = unit.Name;
        unitImage.sprite = victoryPoseSprites[currentVictoryPoseSpriteIndex];

        oldLevelText.text = result.OldLevel + "";
        oldHpText.text = result.OldMaxHp + "";
        oldStrengthText.text = result.OldStrength + "";
        oldAgilityText.text = result.OldAgility + "";
        oldIntelligenceText.text = result.OldIntelligence + "";
        oldVitalityText.text = result.OldVitality + "";
        oldLuckText.text = result.OldLuck + "";
        oldBaseAccuracyText.text = result.OldBaseAccuracy + "";
        oldMagicDefenseText.text = result.OldMagicDefense + "";

        newLevelText.text = result.NewLevel + "";
        newHpText.text = result.NewMaxHp + "";
        newStrengthText.text = result.NewStrength + "";
        newAgilityText.text = result.NewAgility + "";
        newIntelligenceText.text = result.NewIntelligence + "";
        newVitalityText.text = result.NewVitality + "";
        newLuckText.text = result.NewLuck + "";
        newBaseAccuracyText.text = result.NewBaseAccuracy + "";
        newMagicDefenseText.text = result.NewMagicDefense + "";

        newLevelText.color = result.OldLevel == result.NewLevel ? unchangedStatColor : increasedStatColor;
        newHpText.color = result.OldMaxHp == result.NewMaxHp ? unchangedStatColor : increasedStatColor;
        newStrengthText.color = result.OldStrength == result.NewStrength ? unchangedStatColor : increasedStatColor;
        newAgilityText.color = result.OldAgility == result.NewAgility ? unchangedStatColor : increasedStatColor;
        newIntelligenceText.color = result.OldIntelligence == result.NewIntelligence ? unchangedStatColor : increasedStatColor;
        newVitalityText.color = result.OldVitality == result.NewVitality ? unchangedStatColor : increasedStatColor;
        newLuckText.color = result.OldLuck == result.NewLuck ? unchangedStatColor : increasedStatColor;
        newBaseAccuracyText.color = result.OldBaseAccuracy == result.NewBaseAccuracy ? unchangedStatColor : increasedStatColor;
        newMagicDefenseText.color = result.OldMagicDefense == result.NewMagicDefense ? unchangedStatColor : increasedStatColor;
    }
}
