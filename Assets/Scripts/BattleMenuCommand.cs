using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewBattleMenuCommand", menuName = "Final Fantasy/New Battle Menu Command")]
public class BattleMenuCommand : ScriptableObject {

    [SerializeField] string text;
    [SerializeField] PlayerUnitCommand command;

    public string Text => text;
}

public enum PlayerUnitCommand {
    ATTACK,
    WHITE_MAGIC,
    BLACK_MAGIC,
    ITEM,
    RUN
}
