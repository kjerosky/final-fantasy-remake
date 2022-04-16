using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirshipAnimationHandler : MonoBehaviour {

    public Player player;

    public void handleTakeoffCompleted() {
        player.onAirshipTakeoffComplete();
    }

    public void handleLandingCompleted() {
        player.onAirshipLandingComplete();
    }

    public void handleShakingCompleted() {
        player.onAirshipShakingComplete();
    }
}
