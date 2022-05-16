using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionAnimationHandler : MonoBehaviour {

    [SerializeField] Player player;
    [SerializeField] AreaNameDisplay areaNameDisplay;

    public void handleTransitionOutOfSceneStarted() {
        areaNameDisplay.stopDisplay();
    }

    public void handleTransitionIntoSceneCompleted() {
        player.onTransitionIntoSceneComplete();
        areaNameDisplay.startDisplay();
    }
}
