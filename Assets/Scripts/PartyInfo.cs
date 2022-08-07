using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyInfo : MonoBehaviour {

    [SerializeField] List<PlayerUnitBase> playerUnitBases;

    private PlayerUnit[] units;
    private int[] orderIndices = new int[] { 0, 1, 2, 3 };

    void Awake() {
        units = new PlayerUnit[] {
            new PlayerUnit(playerUnitBases[0], "Albert"),
            new PlayerUnit(playerUnitBases[1], "Becca"),
            new PlayerUnit(playerUnitBases[3], "Charles"),
            new PlayerUnit(playerUnitBases[4], "Danielle"),
        };
    }

    public PlayerUnit getUnitAtPosition(int position) {
        int unitIndex = orderIndices[position];
        return units[unitIndex];
    }

    public void swapMemberPositions(int memberPosition1, int memberPosition2) {
        int temp = orderIndices[memberPosition1];
        orderIndices[memberPosition1] = orderIndices[memberPosition2];
        orderIndices[memberPosition2] = temp;
    }

    public int getPartySize() {
        return orderIndices.Length;
    }
}
