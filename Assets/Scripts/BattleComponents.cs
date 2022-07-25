using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleComponents : MonoBehaviour {

    [SerializeField] UnitActionQueue unitActionQueue;
    [SerializeField] BattleMenu battleMenu;

    public UnitActionQueue ActionQueue => unitActionQueue;
    public BattleMenu BattleMenu => battleMenu;
}
