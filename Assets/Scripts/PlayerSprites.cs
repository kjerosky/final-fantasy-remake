using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Player Sprites", fileName = "New PlayerSprites")]
public class PlayerSprites : ScriptableObject {

    [SerializeField] List<Sprite> fighterWalkUpFrames;
    [SerializeField] List<Sprite> fighterWalkDownFrames;
    [SerializeField] List<Sprite> fighterWalkLeftFrames;
    [SerializeField] List<Sprite> fighterWalkRightFrames;

    [SerializeField] List<Sprite> thiefWalkUpFrames;
    [SerializeField] List<Sprite> thiefWalkDownFrames;
    [SerializeField] List<Sprite> thiefWalkLeftFrames;
    [SerializeField] List<Sprite> thiefWalkRightFrames;

    [SerializeField] List<Sprite> monkWalkUpFrames;
    [SerializeField] List<Sprite> monkWalkDownFrames;
    [SerializeField] List<Sprite> monkWalkLeftFrames;
    [SerializeField] List<Sprite> monkWalkRightFrames;

    [SerializeField] List<Sprite> redMageWalkUpFrames;
    [SerializeField] List<Sprite> redMageWalkDownFrames;
    [SerializeField] List<Sprite> redMageWalkLeftFrames;
    [SerializeField] List<Sprite> redMageWalkRightFrames;

    [SerializeField] List<Sprite> whiteMageWalkUpFrames;
    [SerializeField] List<Sprite> whiteMageWalkDownFrames;
    [SerializeField] List<Sprite> whiteMageWalkLeftFrames;
    [SerializeField] List<Sprite> whiteMageWalkRightFrames;

    [SerializeField] List<Sprite> blackMageWalkUpFrames;
    [SerializeField] List<Sprite> blackMageWalkDownFrames;
    [SerializeField] List<Sprite> blackMageWalkLeftFrames;
    [SerializeField] List<Sprite> blackMageWalkRightFrames;

    [SerializeField] List<Sprite> canoeWalkUpFrames;
    [SerializeField] List<Sprite> canoeWalkDownFrames;
    [SerializeField] List<Sprite> canoeWalkLeftFrames;
    [SerializeField] List<Sprite> canoeWalkRightFrames;

    [SerializeField] List<Sprite> shipMoveUpFrames;
    [SerializeField] List<Sprite> shipMoveDownFrames;
    [SerializeField] List<Sprite> shipMoveLeftFrames;
    [SerializeField] List<Sprite> shipMoveRightFrames;

    [SerializeField] List<Sprite> airshipShadowFrames;

    public List<Sprite> FighterWalkUpFrames => fighterWalkUpFrames;
    public List<Sprite> FighterWalkDownFrames => fighterWalkDownFrames;
    public List<Sprite> FighterWalkLeftFrames => fighterWalkLeftFrames;
    public List<Sprite> FighterWalkRightFrames => fighterWalkRightFrames;

    public List<Sprite> ThiefWalkUpFrames => thiefWalkUpFrames;
    public List<Sprite> ThiefWalkDownFrames => thiefWalkDownFrames;
    public List<Sprite> ThiefWalkLeftFrames => thiefWalkLeftFrames;
    public List<Sprite> ThiefWalkRightFrames => thiefWalkRightFrames;

    public List<Sprite> MonkWalkUpFrames => monkWalkUpFrames;
    public List<Sprite> MonkWalkDownFrames => monkWalkDownFrames;
    public List<Sprite> MonkWalkLeftFrames => monkWalkLeftFrames;
    public List<Sprite> MonkWalkRightFrames => monkWalkRightFrames;

    public List<Sprite> RedMageWalkUpFrames => redMageWalkUpFrames;
    public List<Sprite> RedMageWalkDownFrames => redMageWalkDownFrames;
    public List<Sprite> RedMageWalkLeftFrames => redMageWalkLeftFrames;
    public List<Sprite> RedMageWalkRightFrames => redMageWalkRightFrames;

    public List<Sprite> WhiteMageWalkUpFrames => whiteMageWalkUpFrames;
    public List<Sprite> WhiteMageWalkDownFrames => whiteMageWalkDownFrames;
    public List<Sprite> WhiteMageWalkLeftFrames => whiteMageWalkLeftFrames;
    public List<Sprite> WhiteMageWalkRightFrames => whiteMageWalkRightFrames;

    public List<Sprite> BlackMageWalkUpFrames => blackMageWalkUpFrames;
    public List<Sprite> BlackMageWalkDownFrames => blackMageWalkDownFrames;
    public List<Sprite> BlackMageWalkLeftFrames => blackMageWalkLeftFrames;
    public List<Sprite> BlackMageWalkRightFrames => blackMageWalkRightFrames;

    public List<Sprite> CanoeWalkUpFrames => canoeWalkUpFrames;
    public List<Sprite> CanoeWalkDownFrames => canoeWalkDownFrames;
    public List<Sprite> CanoeWalkLeftFrames => canoeWalkLeftFrames;
    public List<Sprite> CanoeWalkRightFrames => canoeWalkRightFrames;

    public List<Sprite> ShipMoveUpFrames => shipMoveUpFrames;
    public List<Sprite> ShipMoveDownFrames => shipMoveDownFrames;
    public List<Sprite> ShipMoveLeftFrames => shipMoveLeftFrames;
    public List<Sprite> ShipMoveRightFrames => shipMoveRightFrames;

    public List<Sprite> AirshipShadowFrames => airshipShadowFrames;
}
