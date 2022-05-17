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

    void Start() {
        TransitionManager transitionManager = FindObjectOfType<TransitionManager>();
        transitionManager.OnStartTransition += () => {
            stopDisplay();
        };
        transitionManager.OnEndTransition += () => {
            startDisplay();
        };
    }

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

        display.transform.localScale = new Vector3(startScale, startScale, startScale);

        Vector3 targetScale = new Vector3(endScale, endScale, endScale);
        while (display.transform.localScale != targetScale) {
            display.transform.localScale = Vector3.MoveTowards(
                display.transform.localScale, targetScale, displayAppearanceRate * Time.deltaTime
            );
            yield return null;
        }
    }
}
