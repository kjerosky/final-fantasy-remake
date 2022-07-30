using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewPlayerUnit", menuName = "Final Fantasy/New Player Unit")]
public class PlayerUnitBase : ScriptableObject {

    [SerializeField] PlayerUnitType unitType;
    [SerializeField] int hp;
    [SerializeField] int strength;
    [SerializeField] int agility;
    [SerializeField] int intelligence;
    [SerializeField] int vitality;
    [SerializeField] int luck;
    [SerializeField] int accuracy;
    [SerializeField] int magicDefense;

    [SerializeField] Sprite battleSpriteStanding;
    [SerializeField] Sprite battleSpriteWalking;
    [SerializeField] Sprite battleSpriteWeaponRaised;
    [SerializeField] Sprite battleSpriteCasting;
    [SerializeField] Sprite battleSpriteKneeling;
    [SerializeField] Sprite battleSpriteDead;
    [SerializeField] Sprite unitActionQueueSprite;

    [SerializeField] List<BattleMenuCommand> battleMenuCommands;

    public PlayerUnitType UnitType => unitType;
    public int Hp => hp;
    public int Strength => strength;
    public int Agility => agility;
    public int Intelligence => intelligence;
    public int Vitality => vitality;
    public int Luck => luck;
    public int Accuracy => accuracy;
    public int MagicDefense => magicDefense;

    public Sprite BattleSpriteStanding => battleSpriteStanding;
    public Sprite BattleSpriteWalking => battleSpriteWalking;
    public Sprite BattleSpriteWeaponRaised => battleSpriteWeaponRaised;
    public Sprite BattleSpriteCasting => battleSpriteCasting;
    public Sprite BattleSpriteKneeling => battleSpriteKneeling;
    public Sprite BattleSpriteDead => battleSpriteDead;
    public Sprite UnitActionQueueSprite => unitActionQueueSprite;

    public List<BattleMenuCommand> BattleMenuCommands => battleMenuCommands;
}

public enum PlayerUnitType {
    FIGHTER,
    THIEF,
    MONK,
    RED_MAGE,
    WHITE_MAGE,
    BLACK_MAGE,
    KNIGHT,
    NINJA,
    MASTER,
    RED_WIZARD,
    WHITE_WIZARD,
    BLACK_WIZARD
}
