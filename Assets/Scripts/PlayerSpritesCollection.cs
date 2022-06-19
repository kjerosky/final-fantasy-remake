using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpritesCollection {

    private Dictionary<PlayerSpritesType, List<List<Sprite>>> typeToSpritesList;

    public PlayerSpritesCollection(PlayerSprites playerSprites) {
        typeToSpritesList = new Dictionary<PlayerSpritesType, List<List<Sprite>>>();

        List<List<Sprite>> fighterSpritesList = new List<List<Sprite>>();
        fighterSpritesList.Add(playerSprites.FighterWalkUpFrames);
        fighterSpritesList.Add(playerSprites.FighterWalkDownFrames);
        fighterSpritesList.Add(playerSprites.FighterWalkLeftFrames);
        fighterSpritesList.Add(playerSprites.FighterWalkRightFrames);
        typeToSpritesList.Add(PlayerSpritesType.FIGHTER, fighterSpritesList);

        List<List<Sprite>> thiefSpritesList = new List<List<Sprite>>();
        thiefSpritesList.Add(playerSprites.ThiefWalkUpFrames);
        thiefSpritesList.Add(playerSprites.ThiefWalkDownFrames);
        thiefSpritesList.Add(playerSprites.ThiefWalkLeftFrames);
        thiefSpritesList.Add(playerSprites.ThiefWalkRightFrames);
        typeToSpritesList.Add(PlayerSpritesType.THIEF, thiefSpritesList);

        List<List<Sprite>> monkSpritesList = new List<List<Sprite>>();
        monkSpritesList.Add(playerSprites.MonkWalkUpFrames);
        monkSpritesList.Add(playerSprites.MonkWalkDownFrames);
        monkSpritesList.Add(playerSprites.MonkWalkLeftFrames);
        monkSpritesList.Add(playerSprites.MonkWalkRightFrames);
        typeToSpritesList.Add(PlayerSpritesType.MONK, monkSpritesList);

        List<List<Sprite>> redMageSpritesList = new List<List<Sprite>>();
        redMageSpritesList.Add(playerSprites.RedMageWalkUpFrames);
        redMageSpritesList.Add(playerSprites.RedMageWalkDownFrames);
        redMageSpritesList.Add(playerSprites.RedMageWalkLeftFrames);
        redMageSpritesList.Add(playerSprites.RedMageWalkRightFrames);
        typeToSpritesList.Add(PlayerSpritesType.RED_MAGE, redMageSpritesList);

        List<List<Sprite>> whiteMageSpritesList = new List<List<Sprite>>();
        whiteMageSpritesList.Add(playerSprites.WhiteMageWalkUpFrames);
        whiteMageSpritesList.Add(playerSprites.WhiteMageWalkDownFrames);
        whiteMageSpritesList.Add(playerSprites.WhiteMageWalkLeftFrames);
        whiteMageSpritesList.Add(playerSprites.WhiteMageWalkRightFrames);
        typeToSpritesList.Add(PlayerSpritesType.WHITE_MAGE, whiteMageSpritesList);

        List<List<Sprite>> blackMageSpritesList = new List<List<Sprite>>();
        blackMageSpritesList.Add(playerSprites.BlackMageWalkUpFrames);
        blackMageSpritesList.Add(playerSprites.BlackMageWalkDownFrames);
        blackMageSpritesList.Add(playerSprites.BlackMageWalkLeftFrames);
        blackMageSpritesList.Add(playerSprites.BlackMageWalkRightFrames);
        typeToSpritesList.Add(PlayerSpritesType.BLACK_MAGE, blackMageSpritesList);

        List<List<Sprite>> canoeSpritesList = new List<List<Sprite>>();
        canoeSpritesList.Add(playerSprites.CanoeWalkUpFrames);
        canoeSpritesList.Add(playerSprites.CanoeWalkDownFrames);
        canoeSpritesList.Add(playerSprites.CanoeWalkLeftFrames);
        canoeSpritesList.Add(playerSprites.CanoeWalkRightFrames);
        typeToSpritesList.Add(PlayerSpritesType.CANOE, canoeSpritesList);

        List<List<Sprite>> shipSpritesList = new List<List<Sprite>>();
        shipSpritesList.Add(playerSprites.ShipMoveUpFrames);
        shipSpritesList.Add(playerSprites.ShipMoveDownFrames);
        shipSpritesList.Add(playerSprites.ShipMoveLeftFrames);
        shipSpritesList.Add(playerSprites.ShipMoveRightFrames);
        typeToSpritesList.Add(PlayerSpritesType.SHIP, shipSpritesList);

        typeToSpritesList.Add(PlayerSpritesType.AIRSHIP, new List<List<Sprite>>() {
            playerSprites.AirshipShadowFrames,
            playerSprites.AirshipShadowFrames,
            playerSprites.AirshipShadowFrames,
            playerSprites.AirshipShadowFrames
        });
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
