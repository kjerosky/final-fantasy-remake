using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AreaNameDisplay : MonoBehaviour {

    [SerializeField] float displaySeconds;
    [SerializeField] float appearanceSeconds;
    [SerializeField] GameObject display;
    [SerializeField] Text displayText;

    private Coroutine sequencePlaying;

    public void stopDisplay() {
        if (sequencePlaying != null) {
            StopCoroutine(sequencePlaying);
        }

        display.SetActive(false);
    }

    public void startDisplay() {
        sequencePlaying = StartCoroutine(displayAreaName());
    }

    public IEnumerator displayAreaName() {
        string areaName = SceneManager.GetActiveScene().name;
        if (areaName == "WorldMap") {
            yield break;
        }

        displayText.text = "";
        display.SetActive(true);

        yield return changeDisplayScale(0f, 1f);
        displayText.text = areaName;

        yield return new WaitForSeconds(displaySeconds);

        displayText.text = "";
        yield return changeDisplayScale(1f, 0f);

        display.SetActive(false);
    }

    private IEnumerator changeDisplayScale(float startScale, float endScale) {
        float displayAppearanceRate = 1 / appearanceSeconds;
        float lowerScaleBound = startScale;
        float upperScaleBound = endScale;
        if (startScale > endScale) {
            displayAppearanceRate = -displayAppearanceRate;
            lowerScaleBound = endScale;
            upperScaleBound = startScale;
        }

        float displayScale = startScale;
        while (displayScale != endScale) {
            display.transform.localScale = new Vector3(displayScale, displayScale, displayScale);
            displayScale += displayAppearanceRate * Time.deltaTime;
            displayScale = Mathf.Clamp(displayScale, lowerScaleBound, upperScaleBound);
            yield return null;
        }

        display.transform.localScale = new Vector3(displayScale, displayScale, displayScale);
    }
}
