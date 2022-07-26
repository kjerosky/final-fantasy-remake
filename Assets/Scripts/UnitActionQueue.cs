using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitActionQueue : MonoBehaviour {

    [SerializeField] List<UnitActionQueueSlot> slots;

    private List<BattleUnit> units;

    public void updateContent(List<BattleUnit> units) {
        this.units = units;

        int disabledSlotsStartingIndex = units.Count;

        for (int i = 0; i < disabledSlotsStartingIndex; i++) {
            slots[i].gameObject.SetActive(true);
            slots[i].updateContent(units[i]);
        }

        for (int i = disabledSlotsStartingIndex; i < slots.Count; i++) {
            slots[i].gameObject.SetActive(false);
        }
    }

    public void showSelectionArrows(List<BattleUnit> unitsNeedingArrows) {
        for (int i = 0; i < slots.Count; i++) {
            slots[i].showSelectionArrow(unitsNeedingArrows.Contains(slots[i].Unit));
        }
    }

    public void showSelectionArrows(List<int> unitIndices) {
        for (int i = 0; i < slots.Count; i++) {
            if (slots[i].gameObject.activeSelf) {
                slots[i].showSelectionArrow(unitIndices.Contains(i));
            }
        }
    }
}
