using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleSetupData : MonoBehaviour {

    public static BattleSetupData Instance { get; private set; }

    private FightType fightType;
    private EnemyFormation enemyFormation;
    private List<EnemyUnitBase> enemyUnitBases;
    private Sprite environmentSprite;

    public FightType FightType => fightType;
    public EnemyFormation EnemyFormation => enemyFormation;
    public List<EnemyUnitBase> EnemyUnitBases => enemyUnitBases;
    public Sprite EnvironmentSprite => environmentSprite;

    void Awake() {
        Instance = this;

        enemyUnitBases = new List<EnemyUnitBase>();
        clear();
    }

    public void clear() {
        fightType = FightType.STANDARD;
        enemyFormation = EnemyFormation.NONE;
        enemyUnitBases.Clear();
        environmentSprite = null;
    }

    public void setup(FightType fightType, EnemyFormation formation, List<EnemyUnitBase> unitBases, Sprite environmentSprite) {
        this.fightType = fightType;
        this.enemyFormation = formation;
        this.enemyUnitBases = unitBases;
        this.environmentSprite = environmentSprite;
    }
}

public enum EnemyFormation {
    NONE,
    SIX_SMALL,
    BOSS_SMALL,
    FIEND,
    CHAOS
}

public enum FightType {
    STANDARD,
    BOSS,
    CHAOS
}
