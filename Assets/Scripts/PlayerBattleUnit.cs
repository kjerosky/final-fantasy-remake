using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PlayerBattleUnit : MonoBehaviour, BattleUnit {

    [SerializeField] Image unitImage;
    [SerializeField] Image unitDeathImage;
    [SerializeField] Text nameText;
    [SerializeField] HpInfo hpInfo;
    [SerializeField] SelectionCursor selectionCursor;
    [SerializeField] GameObject selectionFrameGameObject;
    [SerializeField] BattleComponents battleComponents;
    [SerializeField] float damageKnockbackDistanceX;
    [SerializeField] float damageKnockbackTotalSeconds;

    private PlayerUnit playerUnit;
    private int teamMemberIndex;

    private DamageAnimator damageAnimator;

    private RectTransform unitImageRectTransform;
    private float unitImageStartX;

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

    public void setup(PlayerUnit playerUnit, int teamMemberIndex) {
        this.playerUnit = playerUnit;
        this.teamMemberIndex = teamMemberIndex;

        unitImage.sprite = playerUnit.BattleSpriteStanding;
        unitDeathImage.sprite = playerUnit.BattleSpriteDead;

        nameText.text = playerUnit.Name;

        hpInfo.setHp(playerUnit.CurrentHp, playerUnit.MaxHp);

        unitImageRectTransform = unitImage.GetComponent<RectTransform>();
        unitImageStartX = unitImageRectTransform.anchoredPosition.x;

        damageAnimator = GetComponent<DamageAnimator>();

        commandsCount = playerUnit.BattleMenuCommands.Count;

        actionQueue = battleComponents.ActionQueue;
        battleMenu = battleComponents.BattleMenu;
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
        List<BattleUnit> enemyUnits = battleContext.EnemyBattleUnits;

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

        //TODO PERFORM WALKING AND ATTACK ANIMATIONS

        yield return targetEnemyUnit.takePhysicalDamage(this);

        state = PlayerBattleUnitState.DONE;
    }

    public IEnumerator takePhysicalDamage(BattleUnit attackingUnit) {
        int damage = determinePhysicalDamage(attackingUnit);
        playerUnit.takeDamage(damage);

        if (damage > 0) {
            yield return animateReactionToDamage();
        }

        yield return damageAnimator.animateDamage(damage, playerUnit.CurrentHp, playerUnit.MaxHp);

        if (playerUnit.CurrentHp <= 0) {
            unitDeathImage.enabled = true;
            unitImage.enabled = false;
        }
    }

    private int determinePhysicalDamage(BattleUnit attackingUnit) {
        //TODO REPLACE THIS WITH PROPERLY DETERMINED DAMAGE
        int TEMP_damageTaken = 5;
        return TEMP_damageTaken;
    }

    private IEnumerator animateReactionToDamage() {
        float farRightX = unitImageStartX + damageKnockbackDistanceX;
        float closerRightX = unitImageStartX + damageKnockbackDistanceX / 2;

        Sequence shakeUnit = DOTween.Sequence()
            .Append(unitImageRectTransform
                .DOAnchorPosX(farRightX, damageKnockbackTotalSeconds / 4)
                .SetEase(Ease.Linear)
            ).Append(unitImageRectTransform
                .DOAnchorPosX(unitImageStartX, damageKnockbackTotalSeconds / 4)
                .SetEase(Ease.Linear)
            ).Append(unitImageRectTransform
                .DOAnchorPosX(closerRightX, damageKnockbackTotalSeconds / 4)
                .SetEase(Ease.Linear)
            ).Append(unitImageRectTransform
                .DOAnchorPosX(unitImageStartX, damageKnockbackTotalSeconds / 4)
                .SetEase(Ease.Linear)
            );

        yield return shakeUnit.WaitForCompletion();
    }

    public void setSelected(bool isSelected) {
        selectionCursor.setShowing(isSelected);
    }
}

public enum PlayerBattleUnitState {
    SELECT_COMMAND,
    SELECT_SINGLE_TARGET,
    BUSY,
    DONE
}
