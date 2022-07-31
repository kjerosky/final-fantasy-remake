using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelUpCalculator {

    public static LevelUpResult attemptLevelUp(PlayerUnit playerUnit, int awardedExperience) {
        PlayerUnitType playerUnitType = playerUnit.UnitType;

        LevelUpResult results = new LevelUpResult();
        results.OldLevel = results.NewLevel = playerUnit.Level;
        results.OldMaxHp = results.NewMaxHp = playerUnit.MaxHp;
        results.OldStrength = results.NewStrength = playerUnit.Strength;
        results.OldAgility = results.NewAgility = playerUnit.Agility;
        results.OldIntelligence = results.NewIntelligence = playerUnit.Intelligence;
        results.OldVitality = results.NewVitality = playerUnit.Vitality;
        results.OldLuck = results.NewLuck = playerUnit.Luck;
        results.OldBaseAccuracy = results.NewBaseAccuracy = playerUnit.BaseAccuracy;
        results.OldMagicDefense = results.NewMagicDefense = playerUnit.MagicDefense;
        results.OldLeftoverExperience = results.NewLeftoverExperience = playerUnit.Experience;

        if (!playerUnit.canAct() || playerUnit.Level >= 50) {
            return results;
        }

        int experienceNeededForNextLevel = calculateExperienceNeededForNextLevel(results.OldLevel);
        int totalExperience = playerUnit.Experience + awardedExperience;
        while (totalExperience >= experienceNeededForNextLevel) {
            results.NewMaxHp += determineAdditionalMaxHpForNextLevel(results.NewLevel, playerUnit);
            results.NewStrength += determineAdditionalStrengthForNextLevel(results.NewLevel, playerUnitType);
            results.NewAgility += determineAdditionalAgilityForNextLevel(results.NewLevel, playerUnitType);
            results.NewIntelligence += determineAdditionalIntelligenceForNextLevel(results.NewLevel, playerUnitType);
            results.NewVitality += determineAdditionalVitalityForNextLevel(results.NewLevel, playerUnitType);
            results.NewLuck += determineAdditionalLuckForNextLevel(results.NewLevel, playerUnitType);
            results.NewBaseAccuracy += determineAdditionalBaseAccuracyForNextLevel(playerUnitType);
            results.NewMagicDefense += determineAdditionalMagicDefenseForNextLevel(playerUnitType);
            results.NewLevel++;

            playerUnit.applyLevelUpResults(results);

            totalExperience -= experienceNeededForNextLevel;
            if (results.NewLevel >= 50) {
                break;
            }

            experienceNeededForNextLevel = calculateExperienceNeededForNextLevel(results.NewLevel);
        }

        results.NewLeftoverExperience = totalExperience;

        return results;
    }

    public static int calculateExperienceNeededForNextLevel(int currentLevel) {
        int experienceNeeded = 39 * (currentLevel > 29 ? (29 * 29) : (currentLevel * currentLevel));
        if (currentLevel == 1 || currentLevel == 17 || currentLevel == 22 || (currentLevel > 23 && currentLevel % 2 == 1)) {
            experienceNeeded++;
        }

        return experienceNeeded;
    }

    private static int determineAdditionalMaxHpForNextLevel(int currentLevel, PlayerUnit unit) {
        PlayerUnitType playerUnitType = unit.UnitType;
        int unitVitality = unit.Vitality;

        bool isStrongMaxHpRaise = false;
        if (playerUnitType == PlayerUnitType.FIGHTER || playerUnitType == PlayerUnitType.KNIGHT) {
            isStrongMaxHpRaise = FighterStatGains.isMaxHpGainGuaranteed(currentLevel);
        } else if (playerUnitType == PlayerUnitType.THIEF || playerUnitType == PlayerUnitType.NINJA) {
            isStrongMaxHpRaise = ThiefStatGains.isMaxHpGainGuaranteed(currentLevel);
        } else if (playerUnitType == PlayerUnitType.MONK || playerUnitType == PlayerUnitType.MASTER) {
            isStrongMaxHpRaise = MonkStatGains.isMaxHpGainGuaranteed(currentLevel);
        } else if (playerUnitType == PlayerUnitType.RED_MAGE || playerUnitType == PlayerUnitType.RED_WIZARD) {
            isStrongMaxHpRaise = RedMageStatGains.isMaxHpGainGuaranteed(currentLevel);
        } else if (playerUnitType == PlayerUnitType.WHITE_MAGE || playerUnitType == PlayerUnitType.WHITE_WIZARD) {
            isStrongMaxHpRaise = WhiteMageStatGains.isMaxHpGainGuaranteed(currentLevel);
        } else {
            isStrongMaxHpRaise = BlackMageStatGains.isMaxHpGainGuaranteed(currentLevel);
        }

        if (isStrongMaxHpRaise) {
            return 20 + unitVitality / 4 + BattleCalculator.random(1, 6);
        } else {
            return unitVitality / 4 + 1;
        }
    }

    private static int determineAdditionalStrengthForNextLevel(int currentLevel, PlayerUnitType playerUnitType) {
        bool isGuaranteedToIncrease = false;
        if (playerUnitType == PlayerUnitType.FIGHTER || playerUnitType == PlayerUnitType.KNIGHT) {
            isGuaranteedToIncrease = FighterStatGains.isStrengthGainGuaranteed(currentLevel);
        } else if (playerUnitType == PlayerUnitType.THIEF || playerUnitType == PlayerUnitType.NINJA) {
            isGuaranteedToIncrease = ThiefStatGains.isStrengthGainGuaranteed(currentLevel);
        } else if (playerUnitType == PlayerUnitType.MONK || playerUnitType == PlayerUnitType.MASTER) {
            isGuaranteedToIncrease = MonkStatGains.isStrengthGainGuaranteed(currentLevel);
        } else if (playerUnitType == PlayerUnitType.RED_MAGE || playerUnitType == PlayerUnitType.RED_WIZARD) {
            isGuaranteedToIncrease = RedMageStatGains.isStrengthGainGuaranteed(currentLevel);
        } else if (playerUnitType == PlayerUnitType.WHITE_MAGE || playerUnitType == PlayerUnitType.WHITE_WIZARD) {
            isGuaranteedToIncrease = WhiteMageStatGains.isStrengthGainGuaranteed(currentLevel);
        } else {
            isGuaranteedToIncrease = BlackMageStatGains.isStrengthGainGuaranteed(currentLevel);
        }

        if (isGuaranteedToIncrease) {
            return 1;
        } else {
            return willNotGuaranteedStatIncreaseRandomly() ? 1 : 0;
        }
    }

    private static int determineAdditionalAgilityForNextLevel(int currentLevel, PlayerUnitType playerUnitType) {
        bool isGuaranteedToIncrease = false;
        if (playerUnitType == PlayerUnitType.FIGHTER || playerUnitType == PlayerUnitType.KNIGHT) {
            isGuaranteedToIncrease = FighterStatGains.isAgilityGainGuaranteed(currentLevel);
        } else if (playerUnitType == PlayerUnitType.THIEF || playerUnitType == PlayerUnitType.NINJA) {
            isGuaranteedToIncrease = ThiefStatGains.isAgilityGainGuaranteed(currentLevel);
        } else if (playerUnitType == PlayerUnitType.MONK || playerUnitType == PlayerUnitType.MASTER) {
            isGuaranteedToIncrease = MonkStatGains.isAgilityGainGuaranteed(currentLevel);
        } else if (playerUnitType == PlayerUnitType.RED_MAGE || playerUnitType == PlayerUnitType.RED_WIZARD) {
            isGuaranteedToIncrease = RedMageStatGains.isAgilityGainGuaranteed(currentLevel);
        } else if (playerUnitType == PlayerUnitType.WHITE_MAGE || playerUnitType == PlayerUnitType.WHITE_WIZARD) {
            isGuaranteedToIncrease = WhiteMageStatGains.isAgilityGainGuaranteed(currentLevel);
        } else {
            isGuaranteedToIncrease = BlackMageStatGains.isAgilityGainGuaranteed(currentLevel);
        }

        if (isGuaranteedToIncrease) {
            return 1;
        } else {
            return willNotGuaranteedStatIncreaseRandomly() ? 1 : 0;
        }
    }

    private static int determineAdditionalIntelligenceForNextLevel(int currentLevel, PlayerUnitType playerUnitType) {
        bool isGuaranteedToIncrease = false;
        if (playerUnitType == PlayerUnitType.FIGHTER || playerUnitType == PlayerUnitType.KNIGHT) {
            isGuaranteedToIncrease = FighterStatGains.isIntelligenceGainGuaranteed(currentLevel);
        } else if (playerUnitType == PlayerUnitType.THIEF || playerUnitType == PlayerUnitType.NINJA) {
            isGuaranteedToIncrease = ThiefStatGains.isIntelligenceGainGuaranteed(currentLevel);
        } else if (playerUnitType == PlayerUnitType.MONK || playerUnitType == PlayerUnitType.MASTER) {
            isGuaranteedToIncrease = MonkStatGains.isIntelligenceGainGuaranteed(currentLevel);
        } else if (playerUnitType == PlayerUnitType.RED_MAGE || playerUnitType == PlayerUnitType.RED_WIZARD) {
            isGuaranteedToIncrease = RedMageStatGains.isIntelligenceGainGuaranteed(currentLevel);
        } else if (playerUnitType == PlayerUnitType.WHITE_MAGE || playerUnitType == PlayerUnitType.WHITE_WIZARD) {
            isGuaranteedToIncrease = WhiteMageStatGains.isIntelligenceGainGuaranteed(currentLevel);
        } else {
            isGuaranteedToIncrease = BlackMageStatGains.isIntelligenceGainGuaranteed(currentLevel);
        }

        if (isGuaranteedToIncrease) {
            return 1;
        } else {
            return willNotGuaranteedStatIncreaseRandomly() ? 1 : 0;
        }
    }

    private static int determineAdditionalVitalityForNextLevel(int currentLevel, PlayerUnitType playerUnitType) {
        bool isGuaranteedToIncrease = false;
        if (playerUnitType == PlayerUnitType.FIGHTER || playerUnitType == PlayerUnitType.KNIGHT) {
            isGuaranteedToIncrease = FighterStatGains.isVitalityGainGuaranteed(currentLevel);
        } else if (playerUnitType == PlayerUnitType.THIEF || playerUnitType == PlayerUnitType.NINJA) {
            isGuaranteedToIncrease = ThiefStatGains.isVitalityGainGuaranteed(currentLevel);
        } else if (playerUnitType == PlayerUnitType.MONK || playerUnitType == PlayerUnitType.MASTER) {
            isGuaranteedToIncrease = MonkStatGains.isVitalityGainGuaranteed(currentLevel);
        } else if (playerUnitType == PlayerUnitType.RED_MAGE || playerUnitType == PlayerUnitType.RED_WIZARD) {
            isGuaranteedToIncrease = RedMageStatGains.isVitalityGainGuaranteed(currentLevel);
        } else if (playerUnitType == PlayerUnitType.WHITE_MAGE || playerUnitType == PlayerUnitType.WHITE_WIZARD) {
            isGuaranteedToIncrease = WhiteMageStatGains.isVitalityGainGuaranteed(currentLevel);
        } else {
            isGuaranteedToIncrease = BlackMageStatGains.isVitalityGainGuaranteed(currentLevel);
        }

        if (isGuaranteedToIncrease) {
            return 1;
        } else {
            return willNotGuaranteedStatIncreaseRandomly() ? 1 : 0;
        }
    }

    private static int determineAdditionalLuckForNextLevel(int currentLevel, PlayerUnitType playerUnitType) {
        bool isGuaranteedToIncrease = false;
        if (playerUnitType == PlayerUnitType.FIGHTER || playerUnitType == PlayerUnitType.KNIGHT) {
            isGuaranteedToIncrease = FighterStatGains.isLuckGainGuaranteed(currentLevel);
        } else if (playerUnitType == PlayerUnitType.THIEF || playerUnitType == PlayerUnitType.NINJA) {
            isGuaranteedToIncrease = ThiefStatGains.isLuckGainGuaranteed(currentLevel);
        } else if (playerUnitType == PlayerUnitType.MONK || playerUnitType == PlayerUnitType.MASTER) {
            isGuaranteedToIncrease = MonkStatGains.isLuckGainGuaranteed(currentLevel);
        } else if (playerUnitType == PlayerUnitType.RED_MAGE || playerUnitType == PlayerUnitType.RED_WIZARD) {
            isGuaranteedToIncrease = RedMageStatGains.isLuckGainGuaranteed(currentLevel);
        } else if (playerUnitType == PlayerUnitType.WHITE_MAGE || playerUnitType == PlayerUnitType.WHITE_WIZARD) {
            isGuaranteedToIncrease = WhiteMageStatGains.isLuckGainGuaranteed(currentLevel);
        } else {
            isGuaranteedToIncrease = BlackMageStatGains.isLuckGainGuaranteed(currentLevel);
        }

        if (isGuaranteedToIncrease) {
            return 1;
        } else {
            return willNotGuaranteedStatIncreaseRandomly() ? 1 : 0;
        }
    }

    private static int determineAdditionalBaseAccuracyForNextLevel(PlayerUnitType playerUnitType) {
        if (playerUnitType == PlayerUnitType.FIGHTER || playerUnitType == PlayerUnitType.KNIGHT) {
            return 3;
        } else if (playerUnitType == PlayerUnitType.THIEF || playerUnitType == PlayerUnitType.NINJA) {
            return 2;
        } else if (playerUnitType == PlayerUnitType.MONK || playerUnitType == PlayerUnitType.MASTER) {
            return 3;
        } else if (playerUnitType == PlayerUnitType.RED_MAGE || playerUnitType == PlayerUnitType.RED_WIZARD) {
            return 2;
        } else if (playerUnitType == PlayerUnitType.WHITE_MAGE || playerUnitType == PlayerUnitType.WHITE_WIZARD) {
            return 1;
        } else {
            return 1;
        }
    }

    private static int determineAdditionalMagicDefenseForNextLevel(PlayerUnitType playerUnitType) {
        if (playerUnitType == PlayerUnitType.FIGHTER || playerUnitType == PlayerUnitType.KNIGHT) {
            return 3;
        } else if (playerUnitType == PlayerUnitType.THIEF || playerUnitType == PlayerUnitType.NINJA) {
            return 2;
        } else if (playerUnitType == PlayerUnitType.MONK || playerUnitType == PlayerUnitType.MASTER) {
            return 4;
        } else if (playerUnitType == PlayerUnitType.RED_MAGE || playerUnitType == PlayerUnitType.RED_WIZARD) {
            return 2;
        } else if (playerUnitType == PlayerUnitType.WHITE_MAGE || playerUnitType == PlayerUnitType.WHITE_WIZARD) {
            return 2;
        } else {
            return 2;
        }
    }

    private static bool willNotGuaranteedStatIncreaseRandomly() {
        return BattleCalculator.random(1, 8) <= 2;
    }
}

public class LevelUpResult {
    public int OldLevel { get; set; }
    public int OldMaxHp { get; set; }
    public int OldStrength { get; set; }
    public int OldAgility { get; set; }
    public int OldIntelligence { get; set; }
    public int OldVitality { get; set; }
    public int OldLuck { get; set; }
    public int OldBaseAccuracy { get; set; }
    public int OldMagicDefense { get; set; }
    public int OldLeftoverExperience { get; set; }

    public int NewLevel { get; set; }
    public int NewMaxHp { get; set; }
    public int NewStrength { get; set; }
    public int NewAgility { get; set; }
    public int NewIntelligence { get; set; }
    public int NewVitality { get; set; }
    public int NewLuck { get; set; }
    public int NewBaseAccuracy { get; set; }
    public int NewMagicDefense { get; set; }
    public int NewLeftoverExperience { get; set; }
}
