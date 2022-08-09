using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TravelPoint : MonoBehaviour {

    [SerializeField] TravelDestination destination;
    [SerializeField] bool isInRoom;
    [SerializeField] Vector2 facingDirection;

    public TravelDestination Destination => destination;
    public bool IsInRoom => isInRoom;
    public Vector2 FacingDirection => facingDirection;
    public Vector3 Position => transform.position;
}
