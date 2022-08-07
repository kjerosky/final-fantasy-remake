using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Player Animator Parameters", fileName = "New PlayerAnimatorParameters")]
public class PlayerAnimatorParameters : ScriptableObject {

    [SerializeField] List<Sprite> canoeMovingUpFrames;
    [SerializeField] List<Sprite> canoeMovingDownFrames;
    [SerializeField] List<Sprite> canoeMovingLeftFrames;
    [SerializeField] List<Sprite> canoeMovingRightFrames;
    [SerializeField] List<Sprite> shipMovingUpFrames;
    [SerializeField] List<Sprite> shipMovingDownFrames;
    [SerializeField] List<Sprite> shipMovingLeftFrames;
    [SerializeField] List<Sprite> shipMovingRightFrames;
    [SerializeField] List<Sprite> airshipMovingUpFrames;
    [SerializeField] List<Sprite> airshipMovingDownFrames;
    [SerializeField] List<Sprite> airshipMovingLeftFrames;
    [SerializeField] List<Sprite> airshipMovingRightFrames;
    [SerializeField] float walkFrameTime;
    [SerializeField] float canoeFrameTime;
    [SerializeField] float shipFrameTime;
    [SerializeField] float airshipFrameTime;

    public List<Sprite> CanoeMovingUpFrames => canoeMovingUpFrames;
    public List<Sprite> CanoeMovingDownFrames => canoeMovingDownFrames;
    public List<Sprite> CanoeMovingLeftFrames => canoeMovingLeftFrames;
    public List<Sprite> CanoeMovingRightFrames => canoeMovingRightFrames;
    public List<Sprite> ShipMovingUpFrames => shipMovingUpFrames;
    public List<Sprite> ShipMovingDownFrames => shipMovingDownFrames;
    public List<Sprite> ShipMovingLeftFrames => shipMovingLeftFrames;
    public List<Sprite> ShipMovingRightFrames => shipMovingRightFrames;
    public List<Sprite> AirshipMovingUpFrames => airshipMovingUpFrames;
    public List<Sprite> AirshipMovingDownFrames => airshipMovingDownFrames;
    public List<Sprite> AirshipMovingLeftFrames => airshipMovingLeftFrames;
    public List<Sprite> AirshipMovingRightFrames => airshipMovingRightFrames;
    public float WalkFrameTime => walkFrameTime;
    public float CanoeFrameTime => canoeFrameTime;
    public float ShipFrameTime => shipFrameTime;
    public float AirshipFrameTime => airshipFrameTime;
}
