using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerBattleUnit : MonoBehaviour {

    [SerializeField] PlayerUnitBase playerUnitBase;
    [SerializeField] HpInfo hpInfo;
    [SerializeField] Image playerUnitImage;

    private PlayerUnit playerUnit;

    public void setup() {
        playerUnit = new PlayerUnit(playerUnitBase);

        hpInfo.setHp(playerUnit.CurrentHp, playerUnit.MaxHp);

        playerUnitImage.sprite = playerUnitBase.BattleSpriteStanding;
    }
}
