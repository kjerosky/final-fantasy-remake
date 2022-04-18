using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionAnimationHandler : MonoBehaviour {

    public Player player;

    public void handleTransitionIntoSceneCompleted() {
        player.onTransitionIntoSceneComplete();
    }
}
