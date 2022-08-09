using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkingBattleNpcController : MonoBehaviour, Interactable {

    [SerializeField] Dialog dialog;
    [SerializeField] FightType fightType;
    [SerializeField] EnemyFormation enemyFormation;
    [SerializeField] List<EnemyUnitBase> enemyUnitBases;
    [SerializeField] Sprite battleEnvironmentSprite;
    [SerializeField] GameEvent eventToNotSpawn;

    private CharacterAnimator animator;
    private DialogManager dialogManager;

    void Start() {
        if (GameEventsLog.Instance.isEventLogged(eventToNotSpawn)) {
            Destroy(gameObject);
            return;
        }

        animator = GetComponent<CharacterAnimator>();
        animator.Horizontal = 0;
        animator.Vertical = -1;
        animator.IsMoving = true;
    }

    public bool isCurrentlyInteractable() {
        return true;
    }

    public void interact(Transform initiator) {
        if (dialogManager == null) {
            dialogManager = FindObjectOfType<DialogManager>();
        }

        Vector3 lookDirection = determineLookDirection(initiator.position);
        animator.Horizontal = lookDirection.x;
        animator.Vertical = lookDirection.y;
        animator.IsMoving = false;

        StartCoroutine(dialogManager.showDialog(dialog, () => {
            BattleSetupData.Instance.setup(fightType, enemyFormation, enemyUnitBases, battleEnvironmentSprite);

            FindObjectOfType<BattleTransitionManager>().transitionIntoBattle(transform, eventToNotSpawn);
        }));
    }

    private Vector3 determineLookDirection(Vector3 initiatorPosition) {
        return (initiatorPosition - transform.position).normalized;
    }
}
