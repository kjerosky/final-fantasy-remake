using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEventsLog : MonoBehaviour {

    private HashSet<GameEvent> loggedEvents;

    public static GameEventsLog Instance { get; private set; }

    void Awake() {
        Instance = this;

        loggedEvents = new HashSet<GameEvent>();
    }

    public void logEvent(GameEvent gameEvent) {
        if (gameEvent == GameEvent.NONE) {
            return;
        }

        loggedEvents.Add(gameEvent);
    }

    public bool isEventLogged(GameEvent gameEvent) {
        return loggedEvents.Contains(gameEvent);
    }
}

public enum GameEvent {
    NONE,
    GARLAND_DEFEATED,
    PRINCESS_SARA_RESCUED
}
