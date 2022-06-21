using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyInfo : MonoBehaviour {

    private PlayerSpritesType[] memberTypes = new PlayerSpritesType[] {
        PlayerSpritesType.FIGHTER,
        PlayerSpritesType.THIEF,
        PlayerSpritesType.MONK,
        PlayerSpritesType.RED_MAGE
    };
    private int[] orderIndices = new int[] { 0, 1, 2, 3 };

    public PlayerSpritesType getTypeAtPosition(int position) {
        int memberTypesIndex = orderIndices[position];
        return memberTypes[memberTypesIndex];
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
