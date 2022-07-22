using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitActionQueue : MonoBehaviour {

    [SerializeField] List<UnitActionQueueSlot> slots;

    public void updateContent(List<BattleUnit> units) {
        int disabledSlotsStartingIndex = units.Count;

        for (int i = 0; i < disabledSlotsStartingIndex; i++) {
            slots[i].gameObject.SetActive(true);
            slots[i].updateContent(units[i]);
        }

        for (int i = disabledSlotsStartingIndex; i < slots.Count; i++) {
            slots[i].gameObject.SetActive(false);
        }
    }
}
