using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewPlayerUnit", menuName = "Final Fantasy/New Player Unit")]
public class PlayerUnitBase : ScriptableObject {

    [SerializeField] int hp;
    [SerializeField] int strength;
    [SerializeField] int agility;
    [SerializeField] int intelligence;
    [SerializeField] int vitality;
    [SerializeField] int luck;
    [SerializeField] int hitPercent;
    [SerializeField] int magicDefense;

    [SerializeField] Sprite battleSpriteStanding;
    [SerializeField] Sprite battleSpriteWalking;
    [SerializeField] Sprite battleSpriteWeaponRaised;
    [SerializeField] Sprite battleSpriteKneeling;
    [SerializeField] Sprite battleSpriteDead;
    [SerializeField] Sprite unitActionQueueSprite;

    [SerializeField] List<BattleMenuCommand> battleMenuCommands;

    public int Hp => hp;

    public Sprite BattleSpriteStanding => battleSpriteStanding;
    public Sprite BattleSpriteWalking => battleSpriteWalking;
    public Sprite BattleSpriteWeaponRaised => battleSpriteWeaponRaised;
    public Sprite BattleSpriteKneeling => battleSpriteKneeling;
    public Sprite BattleSpriteDead => battleSpriteDead;
    public Sprite UnitActionQueueSprite => unitActionQueueSprite;

    public List<BattleMenuCommand> BattleMenuCommands => battleMenuCommands;
}
