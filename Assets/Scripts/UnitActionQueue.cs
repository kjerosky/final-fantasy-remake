using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitActionQueue : MonoBehaviour {

    [SerializeField] List<UnitActionQueueSlot> slots;
    [SerializeField] float spacingBetweenSlotCenters;

    public void updateContent(List<BattleUnit> units) {
        int disabledSlotsStartingIndex = units.Count;

        for (int i = 0; i < disabledSlotsStartingIndex; i++) {
            slots[i].gameObject.SetActive(true);
            slots[i].updateContent(units[i]);

            RectTransform slotRectTransform = slots[i].GetComponent<RectTransform>();
            Vector2 slotAnchoredPosition = slotRectTransform.anchoredPosition;
            float newAnchoredPositionX = calculcateAnchoredPositionX(i, units.Count, spacingBetweenSlotCenters);
            slotRectTransform.anchoredPosition = new Vector2(newAnchoredPositionX, slotAnchoredPosition.y);
        }

        for (int i = disabledSlotsStartingIndex; i < slots.Count; i++) {
            slots[i].gameObject.SetActive(false);
        }
    }

    private float calculcateAnchoredPositionX(int unitIndex, int totalUnits, float spacing) {
        return -(spacing * (totalUnits - 1) / 2) + (unitIndex * spacing);
    }
}
