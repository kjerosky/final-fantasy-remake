using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FighterStatGains {

    private static bool[] guaranteedMaxHpGainsByCurrentLevel = new bool[] {
        true,  true,  true,  true,  true,  true,  true,  true,  true,  true,
        false, true,  true,  false, true,  true,  false, true,  true,  false,
        true,  true,  false, false, true,  false, false, true,  false, false,
        true,  false, false, true,  false, false, true,  false, false, true,
        false, false, true,  false, false, true,  false, false, true
    };

    private static bool[] guaranteedAgilityGainsByCurrentLevel = new bool[] {
        true,  true, true,  true, true,  true,  true,  true, true,  true,
        true,  true, true,  true, true,  true,  true,  true, true,  false,
        true,  true, false, true, true,  false, true,  true, false, true,
        false, true, false, true, false, true,  false, true, false, true,
        false, true, false, true, false, true,  false, true, false
    };

    private static bool[] guaranteedIntelligenceGainsByCurrentLevel = new bool[] {
        false, false, true,  false, false, true,  false, false, true,  false,
        false, true,  false, false, true,  false, false, true,  false, false,
        true,  false, false, true,  false, false, true,  false, false, true,
        false, false, true,  false, false, true,  false, false, true,  false,
        false, true,  false, false, false, false, false, false, false
    };

    private static bool[] guaranteedVitalityGainsByCurrentLevel = new bool[] {
        true,  true,  false, true,  true,  false, true,  true,  false, true,
        true,  false, true,  true,  false, true,  true,  false, true,  true,
        false, true,  true,  false, true,  true,  false, true,  true,  false,
        true,  false, true,  false, true,  false, true,  false, true,  false,
        false, false, false, false, false, false, false, false, false
    };

    private static bool[] guaranteedLuckGainsByCurrentLevel = new bool[] {
        false, true,  true,  false, true,  true,  false, true,  true,  false, 
        true,  true,  false, true,  true,  false, true,  true,  false, true, 
        true,  false, true,  true,  false, true,  true,  false, true,  true,
        false, true,  false, true,  false, true,  false, true,  false, false,
        false, false, false, false, false, false, false, false, false
    };

    public static bool isMaxHpGainGuaranteed(int currentLevel) {
        return guaranteedMaxHpGainsByCurrentLevel[currentLevel - 1];
    }

    public static bool isStrengthGainGuaranteed(int currentLevel) {
        return true;
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
