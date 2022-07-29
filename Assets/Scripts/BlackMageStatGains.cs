using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackMageStatGains {

    private static bool[] guaranteedMaxHpGainsByCurrentLevel = new bool[] {
        true, false, true, false, true, false, true, false, false, true,
        false, false, true, false, false, false, true, false, false, false,
        true, false, false, false, true, false, false, false, true, false,
        false, false, false, true, false, false, false, false, false, true,
        false, false, false, false, false, false, false, false, false
    };

    private static bool[] guaranteedStrengthGainsByCurrentLevel = new bool[] {
        false, false, true, false, false, true, false, false, true, false,
        false, true, false, false, true, false, false, true, false, false,
        true, false, false, true, false, false, true, false, false, true,
        false, false, true, false, false, true, false, false, true, false,
        false, false, false, false, false, false, false, false, false
    };

    private static bool[] guaranteedAgilityGainsByCurrentLevel = new bool[] {
        false, true, false, false, true, false, false, true, false, false,
        true, false, false, true, false, false, true, false, false, true,
        false, false, true, false, false, true, false, false, true, false,
        false, true, false, false, true, false, false, true, false, false,
        false, false, false, false, false, false, false, false, false
    };

    private static bool[] guaranteedVitalityGainsByCurrentLevel = new bool[] {
        false, true, false, true, false, true, false, true, false, false,
        true, false, false, false, true, false, false, false, true, false,
        false, false, true, false, false, false, true, false, false, false,
        true, false, false, false, true, false, false, false, true, false,
        false, false, true, false, false, false, true, false, false
    };

    private static bool[] guaranteedLuckGainsByCurrentLevel = new bool[] {
        false, false, true, false, true, false, true, false, true, false,
        false, false, true, false, false, false, true, false, false, false,
        true, false, false, false, true, false, false, false, true, false,
        false, false, true, false, false, false, true, false, false, false,
        true, false, false, false, true, false, false, false, true
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
        return true;
    }

    public static bool isVitalityGainGuaranteed(int currentLevel) {
        return guaranteedVitalityGainsByCurrentLevel[currentLevel - 1];
    }

    public static bool isLuckGainGuaranteed(int currentLevel) {
        return guaranteedLuckGainsByCurrentLevel[currentLevel - 1];
    }
}
