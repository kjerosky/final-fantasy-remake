using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldMapPositionToTransitionData : PositionToTransitionData {

    private static int TEST_TOWN_X = 21;
    private static int TEST_TOWN_Y = 11;

    private Dictionary<Vector2, TransitionData> data;

    void Awake() {
        data = new Dictionary<Vector2, TransitionData>();
        data.Add(new Vector2(151, 95), new TransitionData("Cornelia", TEST_TOWN_X, TEST_TOWN_Y));
        data.Add(new Vector2(151, 96), new TransitionData("Cornelia", TEST_TOWN_X, TEST_TOWN_Y));
        data.Add(new Vector2(151, 97), new TransitionData("Cornelia", TEST_TOWN_X, TEST_TOWN_Y));
        data.Add(new Vector2(154, 95), new TransitionData("Cornelia", TEST_TOWN_X, TEST_TOWN_Y));
        data.Add(new Vector2(154, 96), new TransitionData("Cornelia", TEST_TOWN_X, TEST_TOWN_Y));
        data.Add(new Vector2(154, 97), new TransitionData("Cornelia", TEST_TOWN_X, TEST_TOWN_Y));
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
