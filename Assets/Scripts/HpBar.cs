using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HpBar : MonoBehaviour {

    [SerializeField] GameObject fillBar;

    public void setHp(float normalizedHp) {
        fillBar.transform.localScale = new Vector3(normalizedHp, 1f, 1f);
    }

    public IEnumerator setHpSmooth(float targetNormalizedHp, float changeSeconds) {
        float currentNormalizedHp = fillBar.transform.localScale.x;
        float changeNormalizedHpRate = Mathf.Abs(targetNormalizedHp - currentNormalizedHp) / changeSeconds;

        while (currentNormalizedHp != targetNormalizedHp) {
            currentNormalizedHp = Mathf.MoveTowards(currentNormalizedHp, targetNormalizedHp, changeNormalizedHpRate * Time.deltaTime);
            setHp(currentNormalizedHp);
            yield return null;
        }
    }
}
