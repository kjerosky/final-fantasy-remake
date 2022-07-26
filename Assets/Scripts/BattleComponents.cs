using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleComponents : MonoBehaviour {

    [SerializeField] UnitActionQueue unitActionQueue;
    [SerializeField] BattleMenu battleMenu;
    [SerializeField] BattleMessageBar battleMessageBar;

    public UnitActionQueue ActionQueue => unitActionQueue;
    public BattleMenu BattleMenu => battleMenu;
    public BattleMessageBar BattleMessageBar => battleMessageBar;
}
