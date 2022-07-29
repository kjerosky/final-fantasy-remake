using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedMageStatGains {

    private static bool[] guaranteedMaxHpGainsByCurrentLevel = new bool[] {
        true, false, true, false, true, false, true, true, true, false,
        true, false, true, false, true, true, false, false, false, false,
        false, false, false, false, false, true, false, false, false, false,
        false, false, false, false, false, true, false, false, false, false,
        false, false, false, false, false, true, false, false, false
    };

    private static bool[] guaranteedStrengthGainsByCurrentLevel = new bool[] {
        true, false, true, false, true, false, true, false, true, false,
        true, false, true, false, true, false, true, false, true, false,
        true, false, true, false, true, false, true, false, true, false,
        true, false, true, false, true, false, true, false, true, false,
        true, false, true, false, true, false, true, false, true
    };

    private static bool[] guaranteedAgilityGainsByCurrentLevel = new bool[] {
        false, true, false, true, false, true, false, true, false, false,
        true, false, false, true, false, false, true, false, false, true,
        false, false, true, false, false, true, false, false, true, false,
        false, true, false, false, true, false, false, true, false, false,
        true, false, false, true, false, false, true, false, false
    };

    private static bool[] guaranteedIntelligenceGainsByCurrentLevel = new bool[] {
        false, true, false, true, false, true, false, true, false, true,
        false, true, false, true, false, true, false, true, false, true,
        false, true, false, true, false, true, false, true, false, true,
        false, true, false, true, false, true, false, true, false, true,
        false, true, false, true, false, true, false, true, false
    };

    private static bool[] guaranteedVitalityGainsByCurrentLevel = new bool[] {
        true, true, true, true, true, false, true, false, false, true,
        false, true, false, true, false, true, false, true, false, true,
        false, true, false, false, true, false, false, true, false, false,
        true, false, false, true, false, false, true, false, false, true,
        false, false, true, false, false, true, false, false, true
    };

    private static bool[] guaranteedLuckGainsByCurrentLevel = new bool[] {
        true, false, true, false, true, false, true, false, true, false,
        true, true, false, true, true, false, false, true, true, false,
        false, true, true, false, false, true, true, false, false, true,
        true, false, false, true, true, false, false, true, true, false,
        false, true, true, false, false, true, true, false, false
    };

    public static bool isMaxHpGainGuaranteed(int currentLevel) {
        return guaranteedMaxHpGainsByCurrentLevel[currentLevel - 1];
    }

    public static bool isStrengthGainGuaranteed(int currentLevel) {
        return guaranteedStrengthGainsByCurrentLevel[currentLevel - 1];
    }

    public static bool isAgilityGainGuaranteed(int currentLevel) {
        return guaranteedAgilityGainsByCurrentLevel[currentLevel - 1];
    }

    public static bool isIntelligenceGainGuaranteed(int currentLevel) {
        return guaranteedIntelligenceGainsByCurrentLevel[currentLevel - 1];
    }

    public static bool isVitalityGainGuaranteed(int currentLevel) {
        return guaranteedVitalityGainsByCurrentLevel[currentLevel - 1];
    }

    public static bool isLuckGainGuaranteed(int currentLevel) {
        return guaranteedLuckGainsByCurrentLevel[currentLevel - 1];
    }
}
