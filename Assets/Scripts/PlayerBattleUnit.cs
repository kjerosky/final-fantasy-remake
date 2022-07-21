using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PlayerBattleUnit : MonoBehaviour {

    [SerializeField] PlayerUnitBase playerUnitBase;
    [SerializeField] HpInfo hpInfo;
    [SerializeField] Image playerUnitImage;
    [SerializeField] RectTransform damageNumbers;
    [SerializeField] float damageNumbersFirstPopOffsetY;
    [SerializeField] float damageNumbersSecondPopOffsetY;
    [SerializeField] float takeDamageSeconds;

    private PlayerUnit playerUnit;
    private float damageNumbersStartAnchorPosY;

    public List<BattleMenuCommand> BattleMenuCommands => playerUnit.BattleMenuCommands;

    void Awake() {
        damageNumbersStartAnchorPosY = damageNumbers.anchoredPosition.y;
    }

    public void setup() {
        playerUnit = new PlayerUnit(playerUnitBase);

        hpInfo.setHp(playerUnit.CurrentHp, playerUnit.MaxHp);

        playerUnitImage.sprite = playerUnitBase.BattleSpriteStanding;
    }

    public IEnumerator takeDamagePhysical(EnemyBattleUnit attackingEnemyUnit) {
        int damageTaken = playerUnit.takeDamage(attackingEnemyUnit);

        damageNumbers.gameObject.SetActive(true);
        damageNumbers.GetComponent<Text>().text = damageTaken + "";

        Sequence damageNumbersBouncing = DOTween.Sequence()
            .Append(damageNumbers
                .DOAnchorPosY(damageNumbersStartAnchorPosY + damageNumbersFirstPopOffsetY, takeDamageSeconds / 4)
                .SetEase(Ease.OutQuad)
            ).Append(damageNumbers
                .DOAnchorPosY(damageNumbersStartAnchorPosY, takeDamageSeconds / 4)
                .SetEase(Ease.InQuad)
            ).Append(damageNumbers
                .DOAnchorPosY(damageNumbersStartAnchorPosY + damageNumbersSecondPopOffsetY, takeDamageSeconds / 8)
                .SetEase(Ease.OutQuad)
            ).Append(damageNumbers
                .DOAnchorPosY(damageNumbersStartAnchorPosY, takeDamageSeconds / 8)
                .SetEase(Ease.InQuad)
            );
        damageNumbersBouncing.Play();

        yield return hpInfo.setHpSmooth(playerUnit.CurrentHp, playerUnit.MaxHp, takeDamageSeconds);

        damageNumbers.gameObject.SetActive(false);
    }
}
