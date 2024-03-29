using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUnit : Unit {

    private PlayerUnitBase unitBase;
    private string name;
    private int level;
    private int currentHp;
    private int maxHp;
    private int hitMultiplier;
    private int strength;
    private int agility;
    private int intelligence;
    private int vitality;
    private int luck;
    private int baseAccuracy;
    private int magicDefense;
    private int experience;

    private Weapon weapon;
    public Weapon Weapon {
        get => weapon;
        set => weapon = value;
    }

    public PlayerUnitBase UnitBase => unitBase;
    public string Name => name;
    public int Level => level;
    public int Experience => experience;
    public int TEMP_CurrentHp { set => currentHp = value; }
    public int TEMP_Experience { set => experience = value; }
    public int TEMP_Level { set => level = value; }
    public int CurrentHp => currentHp;
    public int MaxHp => maxHp;
    public int Strength => strength;
    public int Agility => agility;
    public int Intelligence => intelligence;
    public int Vitality => vitality;
    public int Luck => luck;
    public int BaseAccuracy => baseAccuracy;
    public int MagicDefense => magicDefense;
    public PlayerUnitType UnitType => unitBase.UnitType;
    public Sprite BattleSpriteStanding => unitBase.BattleSpriteStanding;
    public Sprite BattleSpriteWalking => unitBase.BattleSpriteWalking;
    public Sprite BattleSpriteWeaponRaised => unitBase.BattleSpriteWeaponRaised;
    public Sprite BattleSpriteCasting => unitBase.BattleSpriteCasting;
    public Sprite BattleSpriteKneeling => unitBase.BattleSpriteKneeling;
    public Sprite BattleSpriteDead => unitBase.BattleSpriteDead;
    public Sprite UnitActionQueueSprite => unitBase.UnitActionQueueSprite;
    public List<BattleMenuCommand> BattleMenuCommands => unitBase.BattleMenuCommands;

    public int NumberOfHits => calculateNumberOfHits();
    public int HitMultiplier => hitMultiplier;
    public int Accuracy => calculateAccuracy();
    public int Evasion => calculateEvasion();
    public int CriticalRate => calculateCriticalRate();
    public int Attack => calculateAttack();
    public int Defense => calculateDefense();

    public PlayerUnit(PlayerUnitBase unitBase, string name) {
        this.unitBase = unitBase;
        this.name = name;

        level = 1;
        maxHp = unitBase.Hp;
        currentHp = maxHp;
        hitMultiplier = 1;
        strength = unitBase.Strength;
        agility = unitBase.Agility;
        intelligence = unitBase.Intelligence;
        vitality = unitBase.Vitality;
        luck = unitBase.Luck;
        baseAccuracy = unitBase.Accuracy;
        magicDefense = unitBase.MagicDefense;
        experience = 0;
    }

    public void takeDamage(int damage) {
        currentHp = Mathf.Max(0, currentHp - damage);
    }

    public int calculateAttack() {
        bool weaponIsEquipped = weapon != null;
        int weaponAttack = weapon == null ? 0 : weapon.Attack;
        int unitStrength = strength;
        bool isMonkOrMaster = UnitType == PlayerUnitType.MONK || UnitType == PlayerUnitType.MASTER;
        bool isBlackMageOrWizard = UnitType == PlayerUnitType.BLACK_MAGE || UnitType == PlayerUnitType.BLACK_WIZARD;

        if (!weaponIsEquipped && isMonkOrMaster) {
            return level * 2;
        } else if (isBlackMageOrWizard || (weaponIsEquipped && isMonkOrMaster)) {
            return weaponAttack + unitStrength / 2 + 1;
        }

        return weaponAttack + unitStrength / 2;
    }

    public int calculateDefense() {
        bool weaponIsEquipped = weapon != null;
        bool isMonkOrMaster = UnitType == PlayerUnitType.MONK || UnitType == PlayerUnitType.MASTER;

        if (!weaponIsEquipped && isMonkOrMaster) {
            return level;
        }

        //TODO RETURN SUM OF UNIT'S EQUIPPED ARMOR DEFENSE RATINGS
        return 0;
    }

    public int calculateAccuracy() {
        int weaponAccuracy = weapon == null ? 0 : weapon.Accuracy;

        return weaponAccuracy + baseAccuracy;
    }

    public int calculateEvasion() {
        //TODO CALCULATE THE SUM OF THE UNIT'S EQUIPPED ARMOR WEIGHT RATINGS
        int totalArmorWeight = 0;

        return 48 + agility + totalArmorWeight;
    }

    public int calculateCriticalRate() {
        bool weaponIsEquipped = weapon != null;
        bool isMonkOrMaster = UnitType == PlayerUnitType.MONK || UnitType == PlayerUnitType.MASTER;

        if (!weaponIsEquipped) {
            if (isMonkOrMaster) {
                return level * 2;
            } else {
                return 0;
            }
        }

        return weapon.CriticalRate;
    }

    public int calculateNumberOfHits() {
        bool weaponIsEquipped = weapon != null;
        bool isMonkOrMaster = UnitType == PlayerUnitType.MONK || UnitType == PlayerUnitType.MASTER;

        int numberOfHits = 1 + Accuracy / 32;
        if (!weaponIsEquipped && isMonkOrMaster) {
            numberOfHits *= 2;
        }

        return numberOfHits;
    }

    public void applyLevelUpResults(LevelUpResult results) {
        level = results.NewLevel;
        maxHp = results.NewMaxHp;
        strength = results.NewStrength;
        agility = results.NewAgility;
        intelligence = results.NewIntelligence;
        vitality = results.NewVitality;
        luck = results.NewLuck;
        baseAccuracy = results.NewBaseAccuracy;
        magicDefense = results.NewMagicDefense;
        experience = results.NewLeftoverExperience;
    }

    public bool canAct() {
        //TODO ACCOUNT FOR OTHER STATUSES HERE TOO
        return currentHp > 0;
    }
}
