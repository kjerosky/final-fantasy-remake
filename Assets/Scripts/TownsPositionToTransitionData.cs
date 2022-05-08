using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TownsPositionToTransitionData : PositionToTransitionData {

    private static string WORLD_MAP_SCENE_NAME = "WorldMap";

    private static int CORNELIA_WORLD_MAP_X = 173;
    private static int CORNELIA_WORLD_MAP_Y = 116;
    
    private Dictionary<Vector2, TransitionData> data;

    void Awake() {
        data = new Dictionary<Vector2, TransitionData>();

        // Cornelia
        data.Add(new Vector2(28, 10), new TransitionData(WORLD_MAP_SCENE_NAME, CORNELIA_WORLD_MAP_X, CORNELIA_WORLD_MAP_Y));
        data.Add(new Vector2(28, 35), new TransitionData(WORLD_MAP_SCENE_NAME, CORNELIA_WORLD_MAP_X, CORNELIA_WORLD_MAP_Y));
        data.Add(new Vector2(44, 22), new TransitionData(WORLD_MAP_SCENE_NAME, CORNELIA_WORLD_MAP_X, CORNELIA_WORLD_MAP_Y));
    }

    public override TransitionData getTransitionDataForTile(float x, float y) {
        TransitionData foundData;
        if (data.TryGetValue(new Vector2((int)x, (int)y), out foundData)) {
            return foundData;
        } else {
            return null;
        }
    }
}
