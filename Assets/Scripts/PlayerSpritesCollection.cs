using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpritesCollection : MonoBehaviour {

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

    private Dictionary<PlayerSpritesType, List<List<Sprite>>> typeToSpritesList;

    void Awake() {
        typeToSpritesList = new Dictionary<PlayerSpritesType, List<List<Sprite>>>();

        List<List<Sprite>> fighterSpritesList = new List<List<Sprite>>();
        fighterSpritesList.Add(fighterWalkUpFrames);
        fighterSpritesList.Add(fighterWalkDownFrames);
        fighterSpritesList.Add(fighterWalkLeftFrames);
        fighterSpritesList.Add(fighterWalkRightFrames);
        typeToSpritesList.Add(PlayerSpritesType.FIGHTER, fighterSpritesList);

        List<List<Sprite>> thiefSpritesList = new List<List<Sprite>>();
        thiefSpritesList.Add(thiefWalkUpFrames);
        thiefSpritesList.Add(thiefWalkDownFrames);
        thiefSpritesList.Add(thiefWalkLeftFrames);
        thiefSpritesList.Add(thiefWalkRightFrames);
        typeToSpritesList.Add(PlayerSpritesType.THIEF, thiefSpritesList);

        List<List<Sprite>> monkSpritesList = new List<List<Sprite>>();
        monkSpritesList.Add(monkWalkUpFrames);
        monkSpritesList.Add(monkWalkDownFrames);
        monkSpritesList.Add(monkWalkLeftFrames);
        monkSpritesList.Add(monkWalkRightFrames);
        typeToSpritesList.Add(PlayerSpritesType.MONK, monkSpritesList);

        List<List<Sprite>> redMageSpritesList = new List<List<Sprite>>();
        redMageSpritesList.Add(redMageWalkUpFrames);
        redMageSpritesList.Add(redMageWalkDownFrames);
        redMageSpritesList.Add(redMageWalkLeftFrames);
        redMageSpritesList.Add(redMageWalkRightFrames);
        typeToSpritesList.Add(PlayerSpritesType.RED_MAGE, redMageSpritesList);

        List<List<Sprite>> whiteMageSpritesList = new List<List<Sprite>>();
        whiteMageSpritesList.Add(whiteMageWalkUpFrames);
        whiteMageSpritesList.Add(whiteMageWalkDownFrames);
        whiteMageSpritesList.Add(whiteMageWalkLeftFrames);
        whiteMageSpritesList.Add(whiteMageWalkRightFrames);
        typeToSpritesList.Add(PlayerSpritesType.WHITE_MAGE, whiteMageSpritesList);

        List<List<Sprite>> blackMageSpritesList = new List<List<Sprite>>();
        blackMageSpritesList.Add(blackMageWalkUpFrames);
        blackMageSpritesList.Add(blackMageWalkDownFrames);
        blackMageSpritesList.Add(blackMageWalkLeftFrames);
        blackMageSpritesList.Add(blackMageWalkRightFrames);
        typeToSpritesList.Add(PlayerSpritesType.BLACK_MAGE, blackMageSpritesList);

        List<List<Sprite>> canoeSpritesList = new List<List<Sprite>>();
        canoeSpritesList.Add(canoeWalkUpFrames);
        canoeSpritesList.Add(canoeWalkDownFrames);
        canoeSpritesList.Add(canoeWalkLeftFrames);
        canoeSpritesList.Add(canoeWalkRightFrames);
        typeToSpritesList.Add(PlayerSpritesType.CANOE, canoeSpritesList);
    }

    public List<List<Sprite>> getSpritesForType(PlayerSpritesType playerSpritesType) {
        return typeToSpritesList[playerSpritesType];
    }
}

public enum PlayerSpritesType {
    FIGHTER,
    THIEF,
    MONK,
    RED_MAGE,
    WHITE_MAGE,
    BLACK_MAGE,
    CANOE,
    SHIP,
    AIRSHIP
}
