using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewEnemyUnit", menuName = "Final Fantasy/New Enemy Unit")]
public class EnemyUnitBase : ScriptableObject {

    [SerializeField] int hp;
    [SerializeField] int gil;
    [SerializeField] int experience;
    [SerializeField] int absorb;
    [SerializeField] int evade;
    [SerializeField] int magicDefense;
    [SerializeField] int damage;
    [SerializeField] int hitPercent;
    [SerializeField] int criticalHitChance;
    [SerializeField] int morale;

    [SerializeField] Sprite battleSprite;

    public int Hp => hp;

    public Sprite BattleSprite => battleSprite;
}
