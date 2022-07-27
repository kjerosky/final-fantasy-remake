using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewEnemyUnit", menuName = "Final Fantasy/New Enemy Unit")]
public class EnemyUnitBase : ScriptableObject {

    [SerializeField] int hp;
    [SerializeField] int gil;
    [SerializeField] int experience;
    [SerializeField] int attack;
    [SerializeField] int accuracy;
    [SerializeField] int numberOfHits;
    [SerializeField] int criticalRate;
    [SerializeField] int defense;
    [SerializeField] int evasion;
    [SerializeField] int magicDefense;
    [SerializeField] int morale;

    [SerializeField] Sprite battleSprite;

    public int Hp => hp;
    public int Gil => gil;
    public int Experience => experience;
    public int Attack => attack;
    public int Accuracy => accuracy;
    public int NumberOfHits => numberOfHits;
    public int CriticalRate => criticalRate;
    public int Defense => defense;
    public int Evasion => evasion;
    public int Morale => morale;

    public Sprite BattleSprite => battleSprite;
}
