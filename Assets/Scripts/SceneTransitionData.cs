using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneTransitionData {

    private static int nextPlayerX = -1;
    private static int nextPlayerY = -1;

    public static void setNextPlayerPosition(int x, int y) {
        nextPlayerX = x;
        nextPlayerY = y;
    }

    public static int getNextPlayerX() {
        return nextPlayerX;
    }

    public static int getNextPlayerY() {
        return nextPlayerY;
    }
}
