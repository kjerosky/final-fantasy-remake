using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneTransitionData : MonoBehaviour {

    private int nextPlayerX = 153;
    private int nextPlayerY = 92;

    void Awake() {
        DontDestroyOnLoad(gameObject);
    }

    public void setNextPlayerPosition(int x, int y) {
        nextPlayerX = x;
        nextPlayerY = y;
    }

    public float getNextPlayerX() {
        return nextPlayerX;
    }

    public float getNextPlayerY() {
        return nextPlayerY;
    }
}
