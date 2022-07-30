using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerBattleUnit : MonoBehaviour, BattleUnit {

    [SerializeField] Image unitImage;
    [SerializeField] Image unitDeathImage;
    [SerializeField] Image weaponRaisedImage;
    [SerializeField] Image weaponStrikingImage;
    [SerializeField] Text nameText;
    [SerializeField] HpInfo hpInfo;
    [SerializeField] SelectionCursor selectionCursor;
    [SerializeField] GameObject selectionFrameGameObject;
    [SerializeField] GameObject statsGameObject;
    [SerializeField] Sprite fistSprite;

    private PlayerUnit playerUnit;
    private int teamMemberIndex;

    private DamageAnimator damageAnimator;
    private PlayerBattleUnitAnimator animator;

    private PlayerBattleUnitState state;

    private BattleContext battleContext;
    private UnitActionQueue actionQueue;
    private BattleMenu battleMenu;

    private int selectedCommandIndex;
    private int commandsCount;
    private PlayerUnitCommand chosenCommand;

    private int selectedEnemyUnitIndex;

    public bool IsEnemy => false;
    public int TeamMemberIndex => teamMemberIndex;
    public Sprite UnitActionQueueSprite => playerUnit.UnitActionQueueSprite;
    public int CurrentHp => playerUnit.CurrentHp;
    public int Level => playerUnit.Level;
    public Unit Unit => playerUnit;

    public void setup(PlayerUnit playerUnit, int teamMemberIndex, Weapon weapon) {
        this.playerUnit = playerUnit;
        this.teamMemberIndex = teamMemberIndex;

        unitImage.sprite = playerUnit.BattleSpriteStanding;
        unitDeathImage.sprite = playerUnit.BattleSpriteDead;

        if (weapon == null) {
            if (playerUnit.UnitType == PlayerUnitType.MONK || playerUnit.UnitType == PlayerUnitType.MASTER) {
                weaponRaisedImage.enabled = false;
                weaponStrikingImage.sprite = fistSprite;
            } else {
                weaponRaisedImage.enabled = false;
                weaponStrikingImage.enabled = false;
            }
        } else {
            weaponRaisedImage.sprite = weapon.BattleSprite;
            weaponStrikingImage.sprite = weapon.BattleSprite;
        }

        nameText.text = playerUnit.Name;

        hpInfo.setHp(playerUnit.CurrentHp, playerUnit.MaxHp);

        damageAnimator = GetComponent<DamageAnimator>();

        animator = GetComponent<PlayerBattleUnitAnimator>();
        animator.setup(playerUnit);

        commandsCount = playerUnit.BattleMenuCommands.Count;

        actionQueue = BattleComponents.Instance.ActionQueue;
        battleMenu = BattleComponents.Instance.BattleMenu;

        setUnitImagesAccordingToStatus();
    }

    private void setUnitImagesAccordingToStatus() {
        //TODO ACCOUNT FOR OTHER STATUSES HERE TOO
        if (playerUnit.CurrentHp <= 0) {
            unitDeathImage.enabled = true;
            unitImage.enabled = false;
        } else if (playerUnit.CurrentHp < playerUnit.MaxHp / 4) {
            unitDeathImage.enabled = false;
            unitImage.enabled = true;
            unitImage.sprite = playerUnit.BattleSpriteKneeling;
        } else {
            unitDeathImage.enabled = false;
            unitImage.enabled = true;
            unitImage.sprite = playerUnit.BattleSpriteStanding;
        }
    }

    public bool canAct() {
        //TODO ACCOUNT FOR OTHER STATUSES HERE TOO
        return playerUnit.CurrentHp > 0;
    }

    public void prepareToAct(BattleContext battleContext) {
        this.battleContext = battleContext;

        state = PlayerBattleUnitState.SELECT_COMMAND;

        selectionFrameGameObject.SetActive(true);
        actionQueue.gameObject.SetActive(true);

        selectedCommandIndex = 0;
        battleMenu.initializeCommands(playerUnit.BattleMenuCommands);
        battleMenu.setShowingCommandsMenu(true);
    }

    public bool act() {
        if (state == PlayerBattleUnitState.SELECT_COMMAND) {
            handleSelectCommand();
        } else if (state == PlayerBattleUnitState.SELECT_SINGLE_TARGET) {
            handleSelectSingleTarget();
        } else if (state == PlayerBattleUnitState.DONE) {
            return true;
        }

        return false;
    }

    private void handleSelectCommand() {
        if (Input.GetKeyDown(KeyCode.W)) {
            selectedCommandIndex--;
            if (selectedCommandIndex < 0) {
                selectedCommandIndex = commandsCount - 1;
            }
        } else if (Input.GetKeyDown(KeyCode.S)) {
            selectedCommandIndex++;
            if (selectedCommandIndex >= commandsCount) {
                selectedCommandIndex = 0;
            }
        } else if (Input.GetKeyDown(KeyCode.Escape)) {
            selectedCommandIndex = 0;
        }

        battleMenu.setSelectedCommand(selectedCommandIndex);

        if (Input.GetKeyDown(KeyCode.Return)) {
            chosenCommand = playerUnit.BattleMenuCommands[selectedCommandIndex].Command;

            if (chosenCommand == PlayerUnitCommand.ATTACK) {
                battleMenu.dimCommandCursor(selectedCommandIndex);

                preparePlayerSelectSingleTarget();
            }
        }
    }

    private void handleSelectSingleTarget() {
        List<EnemyBattleUnit> enemyUnits = battleContext.EnemyBattleUnits;

        if (Input.GetKeyDown(KeyCode.W)) {
            selectedEnemyUnitIndex--;
            if (selectedEnemyUnitIndex < 0) {
                selectedEnemyUnitIndex = enemyUnits.Count - 1;
            }
        } else if (Input.GetKeyDown(KeyCode.S)) {
            selectedEnemyUnitIndex++;
            if (selectedEnemyUnitIndex >= enemyUnits.Count) {
                selectedEnemyUnitIndex = 0;
            }
        }

        for (int i = 0; i < enemyUnits.Count; i++) {
            enemyUnits[i].setSelected(i == selectedEnemyUnitIndex);
        }

        actionQueue.showSelectionArrows(new List<BattleUnit>() {
            enemyUnits[selectedEnemyUnitIndex]
        });

        if (Input.GetKeyDown(KeyCode.Return)) {
            executeCommand();
        } else if (Input.GetKeyDown(KeyCode.Escape)) {
            if (chosenCommand == PlayerUnitCommand.ATTACK) {
                enemyUnits.ForEach(enemy => enemy.setSelected(false));
                battleMenu.brightenCommandCursor(selectedCommandIndex);
                actionQueue.showSelectionArrows(new List<BattleUnit>());
                state = PlayerBattleUnitState.SELECT_COMMAND;
            }
        }
    }

    private void executeCommand() {
        selectionFrameGameObject.SetActive(false);
        actionQueue.gameObject.SetActive(false);
        battleMenu.setShowingCommandsMenu(false);
        battleMenu.brightenCommandCursor(selectedCommandIndex);
        battleContext.EnemyBattleUnits.ForEach(enemy => enemy.setSelected(false));
        battleContext.PlayerBattleUnits.ForEach(playerUnit => playerUnit.setSelected(false));

        if (chosenCommand == PlayerUnitCommand.ATTACK) {
            StartCoroutine(performAttack());
        }
    }

    private void preparePlayerSelectSingleTarget() {
        selectedEnemyUnitIndex = 0;

        state = PlayerBattleUnitState.SELECT_SINGLE_TARGET;
    }


    private IEnumerator performAttack() {
        state = PlayerBattleUnitState.BUSY;

        BattleUnit targetEnemyUnit = battleContext.EnemyBattleUnits[selectedEnemyUnitIndex];

        yield return animator.animateWalkingToAttackPoint();

        StartCoroutine(animator.animateAttacking());
        yield return targetEnemyUnit.takePhysicalDamage(this);

        yield return animator.animateWalkingBackToStartPoint();

        state = PlayerBattleUnitState.DONE;
    }

    public IEnumerator takePhysicalDamage(BattleUnit attackingUnit) {
        DamageCalculationResult result = BattleCalculator.calculatePhysicalDamage(attackingUnit.Unit, playerUnit);
        Debug.Log($"{name} takes: damage={result.Damage} / numberOfHits={result.NumberOfHits} / wasCritical={result.WasCritical}");
        int damage = result.Damage;

        playerUnit.takeDamage(damage);
        if (damage > 0) {
            yield return animator.animateReactionToDamage();
        }

        yield return damageAnimator.animateDamage(damage, playerUnit.CurrentHp, playerUnit.MaxHp);

        setUnitImagesAccordingToStatus();
    }

    public void setSelected(bool isSelected) {
        selectionCursor.setShowing(isSelected);
    }

    public IEnumerator enterBattle() {
        statsGameObject.SetActive(false);
        yield return animator.animateBattleEntrance(canAct());
        statsGameObject.SetActive(true);
    }

    public IEnumerator performVictory() {
        yield return animator.animateVictory(canAct());
    }
}

public enum PlayerBattleUnitState {
    SELECT_COMMAND,
    SELECT_SINGLE_TARGET,
    BUSY,
    DONE
}
