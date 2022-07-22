using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitActionQueueSlot : MonoBehaviour {

    [SerializeField] Image backgroundImage;
    [SerializeField] Image iconImage;
    [SerializeField] Color playerUnitColor;
    [SerializeField] Color enemyUnitColor;
    [SerializeField] List<Sprite> enemyIcons;

    public void updateContent(BattleUnit unit) {
        if (unit.IsEnemyUnit) {
            backgroundImage.color = enemyUnitColor;
            iconImage.sprite = enemyIcons[unit.TeamMemberIndex];
        } else {
            backgroundImage.color = playerUnitColor;
            iconImage.sprite = unit.UnitActionQueueSprite;
        }
    }
}
