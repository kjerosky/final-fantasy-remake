using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PositionToTransitionData : MonoBehaviour {
    public abstract TransitionData getTransitionDataForTile(float x, float y);
}

public class TransitionData {
    public string nextScene { get; }
    public int nextPlayerX { get; }
    public int nextPlayerY { get; }

    public TransitionData(string nextScene, int nextPlayerX, int nextPlayerY) {
        this.nextScene = nextScene;
        this.nextPlayerX = nextPlayerX;
        this.nextPlayerY = nextPlayerY;
    }
}
