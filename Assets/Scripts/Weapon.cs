using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewWeapon", menuName = "Final Fantasy/New Weapon")]
public class Weapon : ScriptableObject {
    [SerializeField] int attack;
    [SerializeField] int accuracy;
    [SerializeField] int criticalRate;
    [SerializeField] int price;
    [SerializeField] List<PlayerUnitType> unitTypesThatCanWield;
    [SerializeField] Sprite battleSprite;
    [SerializeField] WeaponHitType weaponHitType;

    public int Attack => attack;
    public int Accuracy => accuracy;
    public int CriticalRate => criticalRate;
    public Sprite BattleSprite => battleSprite;
    public WeaponHitType WeaponHitType => weaponHitType;
}

public enum WeaponHitType {
    BLUNT,
    SHARP
}
