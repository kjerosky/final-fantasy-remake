using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HpInfo : MonoBehaviour {

    [SerializeField] HpText hpText;
    [SerializeField] HpBar hpBar;

    public void setHp(int currentHp, int maxHp) {
        hpText.setHp(currentHp, maxHp);
        hpBar.setHp((float)currentHp / maxHp);
    }

    public IEnumerator setHpSmooth(int targetHp, int maxHp, float changeSeconds) {
        StartCoroutine(hpText.setHpSmooth(targetHp, maxHp, changeSeconds));

        float targetNormalizedHp = (float)targetHp / maxHp;
        yield return hpBar.setHpSmooth(targetNormalizedHp, changeSeconds);
    }
}
