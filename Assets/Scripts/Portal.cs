using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour {

    [SerializeField] TravelDestination destination;

    public void OnPlayerTriggered() {
        TravelHandler.Instance.travelTo(destination);
    }
}
