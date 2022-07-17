using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpText : MonoBehaviour {

    private Text hpText;

    private int lastSetHp;

    void Awake() {
        hpText = GetComponent<Text>();
    }

    public void setHp(int targetHp, int maxHp) {
        lastSetHp = targetHp;
        hpText.text = $"{lastSetHp}/{maxHp}";
    }

    public IEnumerator setHpSmooth(int targetHp, int maxHp, float changeSeconds) {
        float currentDisplayedHp = lastSetHp;
        float changeHpRate = Mathf.Abs(targetHp - currentDisplayedHp) / changeSeconds;

        while (currentDisplayedHp != targetHp) {
            currentDisplayedHp = Mathf.MoveTowards(currentDisplayedHp, targetHp, changeHpRate * Time.deltaTime);
            setHp(Mathf.RoundToInt(currentDisplayedHp), maxHp);
            yield return null;
        }
    }
}
