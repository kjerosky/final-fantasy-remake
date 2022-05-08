using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldMapPositionToTransitionData : PositionToTransitionData {

    private static int TEST_TOWN_X = 28;
    private static int TEST_TOWN_Y = 11;

    private Dictionary<Vector2, TransitionData> data;

    void Awake() {
        data = new Dictionary<Vector2, TransitionData>();
        data.Add(new Vector2(171, 115), new TransitionData("Cornelia", TEST_TOWN_X, TEST_TOWN_Y));
        data.Add(new Vector2(171, 116), new TransitionData("Cornelia", TEST_TOWN_X, TEST_TOWN_Y));
        data.Add(new Vector2(171, 117), new TransitionData("Cornelia", TEST_TOWN_X, TEST_TOWN_Y));
        data.Add(new Vector2(174, 115), new TransitionData("Cornelia", TEST_TOWN_X, TEST_TOWN_Y));
        data.Add(new Vector2(174, 116), new TransitionData("Cornelia", TEST_TOWN_X, TEST_TOWN_Y));
        data.Add(new Vector2(174, 117), new TransitionData("Cornelia", TEST_TOWN_X, TEST_TOWN_Y));
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
