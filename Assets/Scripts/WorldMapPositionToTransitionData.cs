using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldMapPositionToTransitionData : PositionToTransitionData {

    private Dictionary<Vector2, TransitionData> data;

    void Awake() {
        data = new Dictionary<Vector2, TransitionData>();

        data.Add(new Vector2(129, 127), new TransitionData("TestTown", -1, -1));
        data.Add(new Vector2(129, 128), new TransitionData("TestTown", -1, -1));
        data.Add(new Vector2(129, 129), new TransitionData("TestTown", -1, -1));
        data.Add(new Vector2(132, 127), new TransitionData("TestTown", -1, -1));
        data.Add(new Vector2(132, 128), new TransitionData("TestTown", -1, -1));
        data.Add(new Vector2(132, 129), new TransitionData("TestTown", -1, -1));
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
