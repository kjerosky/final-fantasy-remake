using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirshipAnimationHandler : MonoBehaviour {

    private Player player;

    public void handleTakeoffCompleted() {
        if (player == null) {
            player = FindObjectOfType<Player>();
        }

        player.onAirshipTakeoffComplete();
    }

    public void handleLandingCompleted() {
        if (player == null) {
            player = FindObjectOfType<Player>();
        }

        player.onAirshipLandingComplete();
    }

    public void handleShakingCompleted() {
        if (player == null) {
            player = FindObjectOfType<Player>();
        }

        player.onAirshipShakingComplete();
    }
}
