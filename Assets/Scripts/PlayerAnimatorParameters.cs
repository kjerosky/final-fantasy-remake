using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Player Animator Parameters", fileName = "New PlayerAnimatorParameters")]
public class PlayerAnimatorParameters : ScriptableObject {

    [SerializeField] PlayerSprites playerSprites;
    [SerializeField] float walkFrameTime;
    [SerializeField] float canoeFrameTime;
    [SerializeField] float shipFrameTime;
    [SerializeField] float airshipFrameTime;

    public PlayerSprites PlayerSprites => playerSprites;
    public float WalkFrameTime => walkFrameTime;
    public float CanoeFrameTime => canoeFrameTime;
    public float ShipFrameTime => shipFrameTime;
    public float AirshipFrameTime => airshipFrameTime;
}
