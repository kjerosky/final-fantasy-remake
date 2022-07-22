using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitActionQueueSlot : MonoBehaviour {

    [SerializeField] Image backgroundImage;
    [SerializeField] Image iconImage;
    [SerializeField] Text unitNumberText;
    [SerializeField] Image selectionArrow;
    [SerializeField] Color playerUnitColor;
    [SerializeField] Color enemyUnitColor;
    [SerializeField] Color playerSelectionArrowColor;
    [SerializeField] Color enemySelectionArrowColor;
    [SerializeField] List<Sprite> enemyIcons;

    private BattleUnit unit;

    public void updateContent(BattleUnit unit) {
        this.unit = unit;

        if (unit.IsEnemyUnit) {
            backgroundImage.color = enemyUnitColor;
            iconImage.sprite = enemyIcons[unit.TeamMemberIndex];
            unitNumberText.text = "";
        } else {
            backgroundImage.color = playerUnitColor;
            iconImage.sprite = unit.UnitActionQueueSprite;
            unitNumberText.text = (unit.TeamMemberIndex + 1) + "";
        }

        selectionArrow.gameObject.SetActive(false);
    }

    public void showSelectionArrow(bool isShowing) {
        selectionArrow.gameObject.SetActive(isShowing);
        if (!isShowing) {
            return;
        }

        if (unit.IsEnemyUnit) {
            selectionArrow.color = enemySelectionArrowColor;
        } else {
            selectionArrow.color = playerSelectionArrowColor;
        }
    }
}
