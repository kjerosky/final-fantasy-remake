using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CorneliaPositionToTransitionData : PositionToTransitionData {

    private static int WORLD_MAP_X = 173;
    private static int WORLD_MAP_Y = 116;
    
    private Dictionary<Vector2, TransitionData> data;

    void Awake() {
        data = new Dictionary<Vector2, TransitionData>();
        data.Add(new Vector2(21, 10), new TransitionData("WorldMap", WORLD_MAP_X, WORLD_MAP_Y));
        data.Add(new Vector2(21, 24), new TransitionData("WorldMap", WORLD_MAP_X, WORLD_MAP_Y));
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
