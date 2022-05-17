using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionManager : MonoBehaviour {

    [SerializeField] GameObject transition;
    [SerializeField] GameObject transitionTopBar;
    [SerializeField] GameObject transitionBottomBar;
    [SerializeField] float transitionSeconds;

    public event Action OnStartTransition;
    public event Action OnEndTransition;

    void Start() {
        StartCoroutine(endTransition());
    }

    public IEnumerator startTransition() {
        OnStartTransition?.Invoke();
        transition.SetActive(true);

        yield return changeBarScales(0f, 1f);
    }

    public IEnumerator endTransition() {
        yield return changeBarScales(1f, 0f);

        transition.SetActive(false);
        OnEndTransition?.Invoke();
    }

    private IEnumerator changeBarScales(float startScale, float endScale) {
        float barScaleRate = 1 / transitionSeconds;
        float lowerScaleBound = startScale;
        float upperScaleBound = endScale;
        if (startScale > endScale) {
            barScaleRate = -barScaleRate;
            lowerScaleBound = endScale;
            upperScaleBound = startScale;
        }

        float barScale = startScale;
        while (barScale != endScale) {
            transitionTopBar.transform.localScale = new Vector3(1f, barScale, 1f);
            transitionBottomBar.transform.localScale = new Vector3(1f, barScale, 1f);
            barScale += barScaleRate * Time.deltaTime;
            barScale = Mathf.Clamp(barScale, lowerScaleBound, upperScaleBound);
            yield return null;
        }

        transitionTopBar.transform.localScale = new Vector3(1f, endScale, 1f);
        transitionBottomBar.transform.localScale = new Vector3(1f, endScale, 1f);
    }
}
