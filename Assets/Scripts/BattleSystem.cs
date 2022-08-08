using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class BattleSystem : MonoBehaviour {

    [SerializeField] PlayerBattleUnit playerUnit1;
    [SerializeField] PlayerBattleUnit playerUnit2;
    [SerializeField] PlayerBattleUnit playerUnit3;
    [SerializeField] PlayerBattleUnit playerUnit4;
    [SerializeField] EnemyBattleUnit enemyUnitSmall1;
    [SerializeField] EnemyBattleUnit enemyUnitSmall2;
    [SerializeField] EnemyBattleUnit enemyUnitSmall3;
    [SerializeField] EnemyBattleUnit enemyUnitSmall4;
    [SerializeField] EnemyBattleUnit enemyUnitSmall5;
    [SerializeField] EnemyBattleUnit enemyUnitSmall6;
    [SerializeField] EnemyBattleUnit enemyUnitBossSmall;
    [SerializeField] VictoryComponents victoryComponents;
    [SerializeField] Image environmentImage;

    [SerializeField] PlayerUnitBase playerUnitBase1;
    [SerializeField] PlayerUnitBase playerUnitBase2;
    [SerializeField] PlayerUnitBase playerUnitBase3;
    [SerializeField] PlayerUnitBase playerUnitBase4;
    [SerializeField] EnemyUnitBase enemyUnitBase1;
    [SerializeField] EnemyUnitBase enemyUnitBase2;
    [SerializeField] EnemyUnitBase enemyUnitBase3;
    [SerializeField] EnemyUnitBase enemyUnitBase4;
    [SerializeField] EnemyUnitBase enemyUnitBase5;
    [SerializeField] EnemyUnitBase enemyUnitBase6;
    [SerializeField] EnemyUnitBase enemyUnitBaseBossSmall;
    [SerializeField] Weapon playerWeapon1;
    [SerializeField] Weapon playerWeapon2;
    [SerializeField] Weapon playerWeapon3;
    [SerializeField] Weapon playerWeapon4;

    private UnitActionQueue actionQueue;
    private BattleMenu battleMenu;

    private BattleSystemState state;
    private List<BattleUnit> actionQueueBattleUnits;
    private BattleUnit currentBattleUnit;

    private BattleContext battleContext;
    private List<PlayerBattleUnit> playerBattleUnits;
    private List<EnemyBattleUnit> enemyBattleUnits;

    void Start() {
        victoryComponents.OnEndVictoryProcess += () => {
            exitBattle();
        };

        actionQueue = BattleComponents.Instance.ActionQueue;
        battleMenu = BattleComponents.Instance.BattleMenu;

        List<PlayerUnit> partyUnits;
        PartyInfo partyInfo = FindObjectOfType<PartyInfo>();
        if (partyInfo == null) {
            partyUnits = setupDebugPlayerUnits();
        } else {
            partyUnits = partyInfo.Units;
        }

        playerUnit1.setup(partyUnits[0], 0);
        playerUnit2.setup(partyUnits[1], 1);
        playerUnit3.setup(partyUnits[2], 2);
        playerUnit4.setup(partyUnits[3], 3);

        playerBattleUnits = new List<PlayerBattleUnit>() {
            playerUnit1,
            playerUnit2,
            playerUnit3,
            playerUnit4
        };

        BattleSetupData battleSetupData = BattleSetupData.Instance;
        if (battleSetupData == null) {
            enemyBattleUnits = setupDebugEnemyUnits();
        } else {
            enemyBattleUnits = setupEnemyUnits(battleSetupData);
            environmentImage.sprite = battleSetupData.EnvironmentSprite;
        }

        victoryComponents.setup(playerBattleUnits);

        battleContext = new BattleContext();

        actionQueue.gameObject.SetActive(false);
        battleMenu.setShowingCommandsMenu(false);

        state = BattleSystemState.START;
    }

    void Update() {
        if (state == BattleSystemState.START) {
            StartCoroutine(performStart());
        } else if (state == BattleSystemState.PROCESSING_UNITS) {
            bool currentBattleUnitIsDoneActing = currentBattleUnit.act();
            if (currentBattleUnitIsDoneActing) {
                setupNextActionableUnit();
            }
        } else if (state == BattleSystemState.VICTORY) {
            StartCoroutine(performVictory());
        } else if (state == BattleSystemState.DEFEAT) {
            //TODO
        }
    }

    private void initializeActionQueue() {
        //TODO IMPLEMENT RANDOMIZED ORDER
        actionQueueBattleUnits = playerBattleUnits
            .Cast<BattleUnit>()
            .Concat(enemyBattleUnits
                .Cast<BattleUnit>()
                .ToList()
            ).ToList();

        currentBattleUnit = actionQueueBattleUnits[0];
        if (!currentBattleUnit.canAct()) {
            setupNextActionableUnit();
        } else {
            refreshDataForNextActionableUnit();
        }
    }

    private void setupNextActionableUnit() {
        bool allEnemiesAreGone = enemyBattleUnits
            .All(enemy => enemy.IsEnemy && !enemy.canAct());
        bool allPlayersCannotAct = playerBattleUnits
            .All(playerUnit => !playerUnit.canAct());
        if (allEnemiesAreGone) {
            state = BattleSystemState.VICTORY;
            return;
        } else if (allPlayersCannotAct) {
            Debug.Log("Defeat!");
            state = BattleSystemState.DEFEAT;
            return;
        }

        do {
            actionQueueBattleUnits.RemoveAt(0);
            actionQueueBattleUnits.Add(currentBattleUnit);
            currentBattleUnit = actionQueueBattleUnits[0];
        } while (!currentBattleUnit.canAct());

        refreshDataForNextActionableUnit();
    }

    private void refreshDataForNextActionableUnit() {
        actionQueue.updateContent(actionQueueBattleUnits
            .Where(unit => unit.canAct())
            .ToList()
        );

        initializeBattleContext();
        currentBattleUnit.prepareToAct(battleContext);
    }

    private void initializeBattleContext() {
        List<EnemyBattleUnit> activeEnemyBattleUnits = enemyBattleUnits
            .Where(enemy => enemy.canAct())
            .ToList();

        battleContext.initialize(playerBattleUnits, activeEnemyBattleUnits);
    }

    private IEnumerator performStart() {
        state = BattleSystemState.BUSY;

        List<BattleUnit> activeUnits = playerBattleUnits
            .Cast<BattleUnit>()
            .Concat(enemyBattleUnits)
            .ToList();
        BattleUnit synchronizingBattleUnit = activeUnits[0];
        activeUnits.RemoveAt(0);

        activeUnits.ForEach(unit => StartCoroutine(unit.enterBattle()));
        yield return synchronizingBattleUnit.enterBattle();

        initializeActionQueue();
        state = BattleSystemState.PROCESSING_UNITS;
    }

    private IEnumerator performVictory() {
        state = BattleSystemState.BUSY;

        int playerUnitsThatCanActCount = playerBattleUnits
            .Where(unit => unit.canAct())
            .Count();
        List<EnemyBattleUnit> enemiesYieldingRewards = enemyBattleUnits
            .Where(enemy => enemy.yieldsRewards())
            .ToList();

        int totalGil = 0;
        int totalExperience = 0;
        enemiesYieldingRewards.ForEach(enemy => {
            totalGil += enemy.Gil;
            totalExperience += enemy.Experience;
        });

        int experiencePerUnit = Mathf.Max(1, totalExperience / playerUnitsThatCanActCount);
        List<LevelUpResult> levelUpResults = levelUpPlayerUnits(experiencePerUnit);

        yield return new WaitForSeconds(0.25f);

        StartCoroutine(playerUnit1.performVictory());
        StartCoroutine(playerUnit2.performVictory());
        StartCoroutine(playerUnit3.performVictory());
        StartCoroutine(playerUnit4.performVictory());
        yield return new WaitForSeconds(3f);

        StartCoroutine(victoryComponents.startVictoryProcess(totalGil, experiencePerUnit, playerBattleUnits, levelUpResults));
    }

    private void exitBattle() {
        BattleTransitionManager battleTransitionManager = FindObjectOfType<BattleTransitionManager>();
        if (battleTransitionManager == null) {
            Debug.Log("leaving battle...");
        } else {
            battleTransitionManager.transitionOutOfBattle();
        }
    }

    private List<LevelUpResult> levelUpPlayerUnits(int experiencePerUnit) {
        return playerBattleUnits
            .Select(playerBattleUnit => (PlayerUnit)(playerBattleUnit.Unit))
            .Select(playerUnit =>
                LevelUpCalculator.attemptLevelUp((PlayerUnit)(playerUnit), experiencePerUnit))
            .ToList();
    }

    private List<PlayerUnit> setupDebugPlayerUnits() {
        PlayerUnit unit1 = new PlayerUnit(playerUnitBase1, "Debug_Abraham");
        PlayerUnit unit2 = new PlayerUnit(playerUnitBase2, "Debug_Bobby");
        PlayerUnit unit3 = new PlayerUnit(playerUnitBase3, "Debug_Carly");
        PlayerUnit unit4 = new PlayerUnit(playerUnitBase4, "Debug_Diana");
        unit1.Weapon = playerWeapon1;
        unit2.Weapon = playerWeapon2;
        unit3.Weapon = playerWeapon3;
        unit4.Weapon = playerWeapon4;

        return new List<PlayerUnit>() {
            unit1,
            unit2,
            unit3,
            unit4
        };
    }

    private List<EnemyBattleUnit> setupDebugEnemyUnits() {
        getAllEnemyBattleUnitSlots()
            .ForEach(slot => slot.gameObject.SetActive(false));

        List<EnemyBattleUnit> units = new List<EnemyBattleUnit>();
        if (enemyUnitBase1 != null) {
            enemyUnitSmall1.gameObject.SetActive(true);
            enemyUnitSmall1.setup(new EnemyUnit(enemyUnitBase1), 0);
            units.Add(enemyUnitSmall1);
        }
        if (enemyUnitBase2 != null) {
            enemyUnitSmall2.gameObject.SetActive(true);
            enemyUnitSmall2.setup(new EnemyUnit(enemyUnitBase2), 1);
            units.Add(enemyUnitSmall2);
        }
        if (enemyUnitBase3 != null) {
            enemyUnitSmall3.gameObject.SetActive(true);
            enemyUnitSmall3.setup(new EnemyUnit(enemyUnitBase3), 2);
            units.Add(enemyUnitSmall3);
        }
        if (enemyUnitBase4 != null) {
            enemyUnitSmall4.gameObject.SetActive(true);
            enemyUnitSmall4.setup(new EnemyUnit(enemyUnitBase4), 3);
            units.Add(enemyUnitSmall4);
        }
        if (enemyUnitBase5 != null) {
            enemyUnitSmall5.gameObject.SetActive(true);
            enemyUnitSmall5.setup(new EnemyUnit(enemyUnitBase5), 4);
            units.Add(enemyUnitSmall5);
        }
        if (enemyUnitBase6 != null) {
            enemyUnitSmall6.gameObject.SetActive(true);
            enemyUnitSmall6.setup(new EnemyUnit(enemyUnitBase6), 5);
            units.Add(enemyUnitSmall6);
        }
        if (enemyUnitBaseBossSmall != null) {
            enemyUnitBossSmall.gameObject.SetActive(true);
            enemyUnitBossSmall.setup(new EnemyUnit(enemyUnitBaseBossSmall), 0);
            units.Add(enemyUnitBossSmall);
        }

        return units;
    }

    private List<EnemyBattleUnit> setupEnemyUnits(BattleSetupData battleSetupData) {
        getAllEnemyBattleUnitSlots()
            .ForEach(slot => slot.gameObject.SetActive(false));

        List<EnemyUnitBase> unitBases = battleSetupData.EnemyUnitBases;
        List<EnemyBattleUnit> battleUnits = getEnemyBattleUnitSlotsForFormation(battleSetupData.EnemyFormation);

        List<EnemyBattleUnit> units = new List<EnemyBattleUnit>();
        for (int i = 0; i < unitBases.Count; i++) {
            battleUnits[i].gameObject.SetActive(true);
            battleUnits[i].setup(new EnemyUnit(unitBases[i]), i);
            units.Add(battleUnits[i]);
        }

        return units;
    }

    private List<EnemyBattleUnit> getAllEnemyBattleUnitSlots() {
        return new List<EnemyBattleUnit>() {
            enemyUnitSmall1,
            enemyUnitSmall2,
            enemyUnitSmall3,
            enemyUnitSmall4,
            enemyUnitSmall5,
            enemyUnitSmall6,
            enemyUnitBossSmall
        };
    }

    private List<EnemyBattleUnit> getEnemyBattleUnitSlotsForFormation(EnemyFormation enemyFormation) {
        if (enemyFormation == EnemyFormation.SIX_SMALL) {
            return new List<EnemyBattleUnit>() {
                enemyUnitSmall1,
                enemyUnitSmall2,
                enemyUnitSmall3,
                enemyUnitSmall4,
                enemyUnitSmall5,
                enemyUnitSmall6
            };
        } else if (enemyFormation == EnemyFormation.BOSS_SMALL) {
            return new List<EnemyBattleUnit>() {
                enemyUnitBossSmall
            };
        } else if (enemyFormation == EnemyFormation.FIEND) {
            //TODO
            return null;
        } else if (enemyFormation == EnemyFormation.CHAOS) {
            //TODO
            return null;
        } else {
            return null;
        }
    }
}

public enum BattleSystemState {
    START,
    PROCESSING_UNITS,
    VICTORY,
    DEFEAT,
    BUSY
}
