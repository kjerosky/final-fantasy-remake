using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitActionQueueSlot : MonoBehaviour {

    [SerializeField] Image backgroundImage;
    [SerializeField] Image iconImage;
    [SerializeField] Text unitNumberText;
    [SerializeField] Color playerUnitColor;
    [SerializeField] Color enemyUnitColor;
    [SerializeField] List<Sprite> enemyIcons;

    public void updateContent(BattleUnit unit) {
        if (unit.IsEnemyUnit) {
            backgroundImage.color = enemyUnitColor;
            iconImage.sprite = enemyIcons[unit.TeamMemberIndex];
            unitNumberText.text = "";
        } else {
            backgroundImage.color = playerUnitColor;
            iconImage.sprite = unit.UnitActionQueueSprite;
            unitNumberText.text = (unit.TeamMemberIndex + 1) + "";
        }
    }
}
